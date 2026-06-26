# ChatBot-P3-ST10486804
# CyberBot — Cybersecurity Awareness Chatbot (Part 3 / POE)

## Student Details
- Name: Kitso LITELU
- Student Number: ST10486804
- Date:2026/06/26

---

## Project Description
A WPF-based Cybersecurity Awareness Chatbot built in C# across three parts.
The bot helps users learn about cybersecurity through conversation, topic pages,
a task assistant, a quiz mini-game, NLP simulation and an activity log.

---

## Part 1 Features
- Console-based chatbot with ASCII art header
- Voice greeting on startup
- Menu-driven navigation for Password Safety, Phishing and Safe Browsing
- Typing effect using Thread.Sleep

---

## Part 2 Features
- Full WPF GUI with dark purple theme and gradient design
- Chat bubbles with purple avatars for bot and user
- Mood selection (Happy / Neutral / Sad) at startup
- Sentiment detection — mood badge updates live
- Keyword recognition using Dictionary and delegates
- Random responses using List and Random.Next()
- Memory and Recall — stores user name and favourite topic
- Conversation flow — "tell me more" fetches another tip
- Error handling — unknown input returns polite fallback
- Topic detail pages for Password, Phishing, Browsing, Privacy, Scams, Malware
- Live clock in status bar
- Message counter and rotating Did You Know facts
- Typing indicator using async/await

---

## Part 3 Features

| Feature | Description |
|---------|-------------|
| Task Assistant | Add, view, complete and delete cybersecurity tasks stored in a text file |
| Mini-Game Quiz | 13 cybersecurity questions — multiple choice and true/false — with scoring and instant feedback |
| NLP Simulation | Recognises varied user phrases using a Dictionary of intent keywords |
| Activity Log | Records all bot actions with timestamps — type "show activity log" to view |

---

## How to Access All Features
| Feature | How to open |
|---------|-------------|
| Topic pages | Click any chip: Password, Phishing, Browsing, Privacy, Scams, Malware |
| Task Assistant | Click  My Tasks chip or type "add task" / "my tasks" |
| Quiz | Click  Quiz chip or type "quiz" / "test me" / "play game" |
| Activity Log | Type "show activity log" or "what have you done for me?" |
| NLP reminder | Type "Remind me to update my password tomorrow" |
| NLP task | Type "Add a task to enable two-factor authentication" |

---

## Files

| File | Purpose |
|------|---------|
| ChatBot.cs | Core logic — keywords, memory, sentiment, NLP intents, activity log |
| MainWindow.xaml | WPF main window layout |
| MainWindow.xaml.cs | Code-behind — connects GUI to ChatBot logic |
| TopicWindow.xaml / .cs | Topic detail page with tip navigator |
| TaskWindoww.xaml / .cs | Task Assistant GUI and file storage logic |
| QuizWindow.xaml / .cs | Cybersecurity Quiz mini-game |
| DatabaseHelper.cs | File-based task storage (read, write, update, delete) |
| voice_audio.wav | Voice greeting played on startup |

---

## How to Run
1. Open solution in Visual Studio
2. Ensure target framework is .NET Framework 4.7.2
3. Press F5 to run
4. Select your mood → enter your name → start chatting

---

## YouTube Presentation
[Your YouTube unlisted link here]

---

## GitHub Releases
- v1.0 — Initial release (Part 1 console features)
- v2.0 — GUI release (Part 2 WPF features)
- v3.0 — Final POE (Part 3 Task Assistant, Quiz, NLP, Activity Log)
