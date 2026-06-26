using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBotP2
{
    public delegate string TopicHandler(string input);

    public enum Sentiment { Neutral, Worried, Curious, Frustrated, Happy }

    public class UserMemory
    {
        public string Name { get; set; } = "";
        public string FavouriteTopic { get; set; } = "";
        public string LastTopic { get; set; } = "";
    }

    public class ChatBot
    {
        public UserMemory Memory { get; } = new UserMemory();

        private readonly Random _rng = new Random();

      
        private readonly List<string> _activityLog = new List<string>();

        public void LogAction(string description)
        {
            string entry = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm"), description);
            _activityLog.Add(entry);
            
            if (_activityLog.Count > 10)
                _activityLog.RemoveAt(0);
        }

        public string GetActivityLog()
        {
            if (_activityLog.Count == 0)
                return "No actions recorded yet. Try adding a task, taking the quiz, or asking about a cybersecurity topic!";

            string log = "Here is a summary of recent actions:";
            for (int i = 0; i < _activityLog.Count; i++)
                log += string.Format("{0}. {1}", i + 1, _activityLog[i]);
            return log.TrimEnd();
        }

        // NLP intent dictionary - maps intent names to lists of phrase variations
        // This allows the bot to understand the same request worded differently
        // e.g. "add task", "create task", "i need to" all map to the same intent

        private readonly Dictionary<string, List<string>> _nlpIntents =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["task_add"] = new List<string>
                {
                    "add task", "add a task", "create task", "new task",
                    "set a task", "i need to", "remind me to", "can you remind",
                    "add reminder", "set reminder", "set a reminder", "remind me",
                    "enable 2fa", "enable two factor", "review privacy",
                    "update password", "check privacy", "backup"
                },
                ["task_view"] = new List<string>
                {
                    "my tasks", "show tasks", "view tasks", "list tasks",
                    "what are my tasks", "show my tasks", "task list"
                },
                ["quiz"] = new List<string>
                {
                    "quiz", "test me", "play game", "mini game",
                    "start quiz", "take quiz", "cybersecurity test", "play"
                },
                ["activity_log"] = new List<string>
                {
                    "activity log", "show log", "what have you done",
                    "what have you done for me", "recent actions",
                    "show activity", "history", "action log", "what did you do"
                },
                ["greeting"] = new List<string>
                {
                    "hello", "hi", "hey", "good morning", "good afternoon",
                    "good evening", "howzit", "sup", "greetings"
                },
                ["how_are_you"] = new List<string>
                {
                    "how are you", "how r u", "hows it going",
                    "are you okay", "you okay", "how do you do"
                }
            };

       
        public string DetectIntent(string input)
        {
            string lower = input.ToLower().Trim();
            foreach (var intent in _nlpIntents)
            {
                foreach (string phrase in intent.Value)
                {
                    if (lower.Contains(phrase))
                        return intent.Key;
                }
            }
            return "unknown";
        }

        private readonly Dictionary<string, List<string>> _responses;
        private readonly Dictionary<string, TopicHandler> _handlers;
        private readonly Dictionary<Sentiment, List<string>> _sentimentWords;
        private readonly List<string> _moreWords = new List<string>
            { "more", "another", "tell me more", "another tip", "keep going", "continue", "explain further" };

        public ChatBot()
        {
            _responses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = new List<string>
                {

                    "Never reuse passwords across different sites.",
                    "Enable two-factor authentication  wherever possible for an extra layer of security.",
                    "Avoid using personal info like your name, birthday, or pet's name in passwords.",
                    "A strong password has at least 12 characters: uppercase, lowercase, numbers and symbols. Example: @KI32We$$"
                },
                ["phishing"] = new List<string>
                {
                    "Be cautious of emails asking for personal information as legitimate companies rarely do this.",
                    "Check the sender's actual email address, not just the display name.",
                    "Scammers often create urgency 'Act now!' is a major red flag.",
                    "When in doubt, go directly to the company's official website rather than clicking a link."
                },
                ["safe browsing"] = new List<string>
                {
                    "Always look for HTTPS and the padlock icon before entering any personal details.",
                    "Keep your browser and extensions up to date to patch security vulnerabilities.",
                    "Avoid public Wi-Fi for banking or sensitive tasks . Please use a VPN if you must.",
                    "Clear your cookies and cache regularly to reduce tracking."
                },
                ["privacy"] = new List<string>
                {
                    "Review app permissions regularly  and check if it does that flashlight app really need your contacts?",
                    "Use a privacy-focused search engine like DuckDuckGo to reduce data collection.",
                    "Check your social media privacy settings at least every six months.",
                    "Be mindful of what personal information you share publicly online.",
                    "Read privacy policies before signing up for new services."
                },
                ["scam"] = new List<string>
                {
                    "If an offer sounds too good to be true, it almost certainly is.",
                    "Never send money or gift cards to someone you have not met in person.",
                    "Verify requests by calling the organisation directly using their official number.",
                    "Scammers may impersonate banks, government agencies, or even family members.",
                    "Report scams to your country's consumer protection authority."
                },
                ["malware"] = new List<string>
                {
                    "Only download software from official or well-known trusted sources.",
                    "Keep your operating system and antivirus software updated at all times.",
                    "Do not plug in unknown USB drives — they can auto-run malicious code.",
                    "Ransomware can encrypt your files; regular offline backups are your best defence.",
                    "If your device behaves strangely, run a full antivirus scan immediately."
                }
            };

            _sentimentWords = new Dictionary<Sentiment, List<string>>
            {
                [Sentiment.Worried] = new List<string> { "worried", "scared", "afraid", "anxious", "unsafe", "vulnerable", "fear", "nervous" },
                [Sentiment.Curious] = new List<string> { "curious", "wondering", "interested", "tell me", "how does", "what is", "explain", "learn" },
                [Sentiment.Frustrated] = new List<string> { "frustrated", "annoyed", "confused", "complicated", "difficult", "lost", "hard", "don't understand" },
                [Sentiment.Happy] = new List<string> { "great", "thanks", "awesome", "helpful", "love", "perfect", "cool", "good" }
            };

            _handlers = new Dictionary<string, TopicHandler>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = input => GetRandom("password"),
                ["phishing"] = input => GetRandom("phishing"),
                ["safe browsing"] = input => GetRandom("safe browsing"),
                ["browsing"] = input => GetRandom("safe browsing"),
                ["privacy"] = input => GetRandom("privacy"),
                ["scam"] = input => GetRandom("scam"),
                ["malware"] = input => GetRandom("malware"),
                ["virus"] = input => GetRandom("malware"),
                ["2fa"] = input => "Two-Factor Authentication  adds a second step , usually a code sent to your phone — making it much harder for attackers to access your account even with your password.",
                ["vpn"] = input => "A VPN encrypts your internet traffic, hiding your activity from your ISP and others on the same network. Especially useful on public Wi-Fi.",
                ["ransomware"] = input => "Ransomware encrypts your files and demands payment. Prevention: keep backups offline, never open unexpected email attachments, keep software updated.",
                ["antivirus"] = input => "Keep your antivirus updated and run full scans weekly. Windows Defender is decent; Malwarebytes is a strong free complement.",
            };
        }

        public string Respond(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "I did not catch that — could you please type something?";

            string input = userInput.Trim().ToLower();
            Sentiment sentiment = DetectSentiment(input);

          
            string intent = DetectIntent(input);

            if (intent == "activity_log")
            {
                LogAction("User viewed activity log");
                return GetActivityLog();
            }

            if (intent == "greeting")
            {
                string name = string.IsNullOrEmpty(Memory.Name) ? "" : ", " + Memory.Name;
                return "Hello" + name + "! How can I help you stay safe online today?";
            }

            if (intent == "how_are_you")
                return "I am doing great, thank you for asking! I am always ready to help you stay safe online. What can I help you with?";

            if (intent == "task_add" && (input.Contains("remind") || input.Contains("tomorrow") || input.Contains("next week")))
            {
                string subject = input;
                foreach (string phrase in new[] { "can you remind me to", "remind me to", "remind me", "add a reminder to", "add reminder to" })
                {
                    if (input.Contains(phrase))
                    {
                        int idx = input.IndexOf(phrase) + phrase.Length;
                        subject = input.Substring(idx).Trim();
                        break;
                    }
                }
                
                string timeframe = "unspecified time";
                if (input.Contains("tomorrow")) timeframe = "tomorrow";
                else if (input.Contains("next week")) timeframe = "next week";
                else if (input.Contains("in "))
                {
                    int tidx = input.IndexOf("in ");
                    timeframe = input.Substring(tidx).Trim();
                }

                string taskTitle = subject.Length > 0
                    ? char.ToUpper(subject[0]) + subject.Substring(1)
                    : "Security task";

                LogAction(string.Format("Reminder set: '{0}' on {1}", taskTitle, timeframe));
                return string.Format("Reminder set for '{0}' on {1}. I have logged this action for you.", taskTitle, timeframe);
            }

 
            if (intent == "task_add")
            {
                string subject = input;
                foreach (string phrase in new[] { "add a task to", "add task to", "add a task", "create a task to", "i need to", "set a task to" })
                {
                    if (input.Contains(phrase))
                    {
                        int idx = input.IndexOf(phrase) + phrase.Length;
                        subject = input.Substring(idx).Trim();
                        break;
                    }
                }
                string taskTitle = subject.Length > 0
                    ? char.ToUpper(subject[0]) + subject.Substring(1)
                    : "New cybersecurity task";

                LogAction(string.Format("Task recognised via NLP: '{0}'", taskTitle));
                return string.Format("Task recognised: '{0}'. Would you like to set a reminder for this task? You can also click 'My Tasks' to manage it.", taskTitle);
            }

            
            if (input.StartsWith("my name is "))
            {
                string name = Capitalise(input.Substring(11).Trim().Split(' ')[0]);
                Memory.Name = name;
                return "Nice to meet you, " + name + "! You can ask me anything regarding password safety, phishing, malware, privacy, scams, safe browsing and more.";
            }

            
            if (input.Contains("i'm interested in") || input.Contains("i am interested in") || input.Contains("my favourite topic is"))
            {
                string topic = ExtractInterest(input);
                if (!string.IsNullOrEmpty(topic))
                {
                    Memory.FavouriteTopic = topic;
                    return "Great! I will remember that you are interested in " + topic + ". It is a crucial part of staying safe online. Would you like a tip?";
                }
            }

            
            if (_moreWords.Any(k => input.Contains(k)))
            {
                if (!string.IsNullOrEmpty(Memory.LastTopic))
                    return GetRandom(Memory.LastTopic);
                return "Sure! Which topic would you like more on — password, phishing, privacy, scams, malware, or safe browsing?";
            }

            
            if (input == "exit" || input == "bye" || input == "quit" || input.Contains("goodbye"))
            {
                string n = string.IsNullOrEmpty(Memory.Name) ? "there" : Memory.Name;
                return "Goodbye, " + n + "! And remember to stay safe online";
            }

            
            if (input == "help" || input == "menu" || input == "topics")
                return "I can help with:\n• Password Safety\n• Phishing\n• Safe Browsing\n• Privacy\n• Scams\n• Malware\n• 2FA, VPN, Ransomware, Antivirus\n\nType a keyword or click a button!";

            
            if (input.Contains("remember") && !string.IsNullOrEmpty(Memory.FavouriteTopic))
            {
                string n = string.IsNullOrEmpty(Memory.Name) ? "You" : Memory.Name;
                return n + ", you mentioned you are interested in " + Memory.FavouriteTopic + ". Here is a tip: " + GetRandom(Memory.FavouriteTopic);
            }

           
            foreach (var entry in _handlers)
            {
                if (input.Contains(entry.Key))
                {
                    Memory.LastTopic = entry.Key;
                    string response = entry.Value(input);
                    return WrapSentiment(response, sentiment);
                }
            }

            
            string name2 = string.IsNullOrEmpty(Memory.Name) ? "" : ", " + Memory.Name;
            return "I am not sure I understand that" + name2 + ". Could you try rephrasing?\nTry typing 'help' to see all available topics.";
        }

        public Sentiment DetectSentiment(string input)
        {
            foreach (var entry in _sentimentWords)
                if (entry.Value.Any(k => input.Contains(k)))
                    return entry.Key;
            return Sentiment.Neutral;
        }

        private string WrapSentiment(string response, Sentiment s)
        {
            switch (s)
            {
                case Sentiment.Worried: return "It is completely understandable to feel that way. Here is something that can help:\n\n" + response;
                case Sentiment.Curious: return "Great question! Here is what you should know:\n\n" + response;
                case Sentiment.Frustrated: return "No worries,let me break it down simply:\n\n" + response;
                case Sentiment.Happy: return "Glad you are engaged! Here is a tip:\n\n" + response;
                default: return response;
            }
        }

        private string GetRandom(string topic)
        {
            if (_responses.TryGetValue(topic, out var pool) && pool.Count > 0)
                return pool[_rng.Next(pool.Count)];
            return "That is an important  topic. Could you be more specific?";
        }

        private string ExtractInterest(string input)
        {
            string[] prefixes = { "i'm interested in ", "i am interested in ", "my favourite topic is " };
            foreach (string p in prefixes)
            {
                int idx = input.IndexOf(p, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                    return input.Substring(idx + p.Length).Trim().TrimEnd('.', '!', '?');
            }
            return "";
        }

        private static string Capitalise(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}

















