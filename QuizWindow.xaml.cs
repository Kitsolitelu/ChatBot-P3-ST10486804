using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChatBotP2
{
   
    public class QuizQuestion
    {
        public string Type        { get; set; }  
        public string Question    { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex   { get; set; }  
        public string Explanation { get; set; }  
    }

    public partial class QuizWindow : Window
    {
        private List<QuizQuestion> _questions;
        private int  _currentIndex = 0;
        private int  _score        = 0;
        private bool _answered     = false;

        
        private static readonly Color CorrectColor = Color.FromRgb(34,  197, 94);
        private static readonly Color WrongColor   = Color.FromRgb(239, 68,  68);
        private static readonly Color PurpleColor  = Color.FromRgb(110, 64,  201);

        public QuizWindow()
        {
            InitializeComponent();
            BuildQuestions();
            ShowQuestion();
        }

        // Builds all 13 quiz questions using a List<QuizQuestion>
        // Each question has a type (Multiple Choice or True/False),
        // a list of options, the correct answer index and an explanation
        // Questions are shuffled using Fisher-Yates algorithm for variety
        private void BuildQuestions()
        {
            
            _questions = new List<QuizQuestion>
            {
                
                new QuizQuestion
                {
                    Type     = "Multiple Choice",
                    Question = "What should you do if you receive an email asking for your password?",
                    Options  = new List<string> { "A)  Reply with your password", "B)  Delete the email", "C)  Report the email as phishing", "D)  Ignore it" },
                    CorrectIndex = 2,
                    Explanation  = "Correct! Reporting phishing emails helps prevent scams and protects other users."
                },
                new QuizQuestion
                {
                    Type     = "Multiple Choice",
                    Question = "Which of the following is an example of a strong password?",
                    Options  = new List<string> { "A)  password123", "B)  @K!32We$$mX9", "C)  yourname2000", "D)  qwerty" },
                    CorrectIndex = 1,
                    Explanation  = "Strong passwords use uppercase, lowercase, numbers and symbols — and are at least 12 characters long."
                },
                new QuizQuestion
                {
                    Type     = "Multiple Choice",
                    Question = "What does HTTPS stand for?",
                    Options  = new List<string> { "A)  Hyper Text Transfer Protocol Secure", "B)  High Tech Transfer Protocol System", "C)  Hyper Transfer Technology Protocol Suite", "D)  Home Transfer Text Protocol Secure" },
                    CorrectIndex = 0,
                    Explanation  = "HTTPS means the connection between your browser and the website is encrypted — always look for the padlock icon."
                },
                new QuizQuestion
                {
                    Type     = "Multiple Choice",
                    Question = "What is malware?",
                    Options  = new List<string> { "A)  A type of antivirus software", "B)  Malicious software designed to damage or gain unauthorised access", "C)  A secure web browser", "D)  A firewall tool" },
                    CorrectIndex = 1,
                    Explanation  = "Malware includes viruses, ransomware, spyware and trojans — always keep antivirus software updated."
                },
                new QuizQuestion
                {
                    Type     = "Multiple Choice",
                    Question = "Which of these is a sign of a phishing email?",
                    Options  = new List<string> { "A)  It comes from a known contact", "B)  It has a personalised greeting with your name", "C)  It creates urgency and asks you to click a link immediately", "D)  It has no attachments" },
                    CorrectIndex = 2,
                    Explanation  = "Phishing emails often create urgency — 'Act now or your account will be closed!' Always verify before clicking any link."
                },
                new QuizQuestion
                {
                    Type     = "Multiple Choice",
                    Question = "What is two-factor authentication (2FA)?",
                    Options  = new List<string> { "A)  Using two different passwords", "B)  A second verification step like a code sent to your phone", "C)  Logging in from two devices", "D)  Having two email accounts" },
                    CorrectIndex = 1,
                    Explanation  = "2FA adds a second layer of security — even if someone has your password, they cannot access your account without the second factor."
                },
                new QuizQuestion
                {
                    Type     = "Multiple Choice",
                    Question = "What should you do on public Wi-Fi?",
                    Options  = new List<string> { "A)  Do your online banking as normal", "B)  Share sensitive files freely", "C)  Use a VPN to encrypt your connection", "D)  Disable your firewall" },
                    CorrectIndex = 2,
                    Explanation  = "Public Wi-Fi is not secure — a VPN encrypts your traffic so others on the same network cannot spy on you."
                },
                new QuizQuestion
                {
                    Type     = "Multiple Choice",
                    Question = "What does a VPN do?",
                    Options  = new List<string> { "A)  Speeds up your internet connection", "B)  Blocks all websites", "C)  Encrypts your internet traffic and hides your IP address", "D)  Scans for viruses" },
                    CorrectIndex = 2,
                    Explanation  = "A VPN (Virtual Private Network) creates an encrypted tunnel for your data — essential for privacy on public networks."
                },

                
                new QuizQuestion
                {
                    Type     = "True / False",
                    Question = "You should use the same password for all your accounts to make it easier to remember.",
                    Options  = new List<string> { "A)  True", "B)  False" },
                    CorrectIndex = 1,
                    Explanation  = "False! If one account is hacked, using the same password means ALL your accounts are at risk. Use a password manager instead."
                },
                new QuizQuestion
                {
                    Type     = "True / False",
                    Question = "Clicking a link in an unexpected email from your bank is safe as long as it looks official.",
                    Options  = new List<string> { "A)  True", "B)  False" },
                    CorrectIndex = 1,
                    Explanation  = "False! Phishing emails are designed to look official. Always go directly to the bank website by typing the address yourself."
                },
                new QuizQuestion
                {
                    Type     = "True / False",
                    Question = "Keeping your operating system and apps updated helps protect against security vulnerabilities.",
                    Options  = new List<string> { "A)  True", "B)  False" },
                    CorrectIndex = 0,
                    Explanation  = "True! Updates patch known security vulnerabilities that hackers actively exploit. Always update when prompted."
                },
                new QuizQuestion
                {
                    Type     = "True / False",
                    Question = "A padlock icon in your browser always means a website is completely safe and trustworthy.",
                    Options  = new List<string> { "A)  True", "B)  False" },
                    CorrectIndex = 1,
                    Explanation  = "False! The padlock means the connection is encrypted, but it does NOT mean the website itself is legitimate. Phishing sites can also have HTTPS."
                },
                new QuizQuestion
                {
                    Type     = "True / False",
                    Question = "Social engineering attacks rely on tricking people rather than hacking software.",
                    Options  = new List<string> { "A)  True", "B)  False" },
                    CorrectIndex = 0,
                    Explanation  = "True! Social engineering exploits human psychology — impersonating trusted people to manipulate victims into revealing sensitive information."
                }
            };

            
            var rng = new Random();
            for (int i = _questions.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                QuizQuestion temp = _questions[i];
                _questions[i]     = _questions[j];
                _questions[j]     = temp;
            }
        }

        
        private void ShowQuestion()
        {
            if (_currentIndex >= _questions.Count)
            {
                ShowFinalScore();
                return;
            }

            _answered = false;
            QuizQuestion q = _questions[_currentIndex];

            
            QuestionTypeLabel.Text = q.Type.ToUpper();
            QuestionTypeBadge.Background = q.Type == "True / False"
                ? new SolidColorBrush(Color.FromRgb(14, 116, 144))
                : new SolidColorBrush(Color.FromRgb(45, 31, 78));

            
            QuestionText.Text = string.Format("Q{0}. {1}", _currentIndex + 1, q.Question);

            
            ScoreLabel.Text = string.Format("{0} / {1}", _score, _currentIndex);

           
            double progress = (double)_currentIndex / _questions.Count;
            ProgressFill.Width = progress * 760;
            ProgressLabel.Text = string.Format("{0} of {1}", _currentIndex + 1, _questions.Count);

            
            FeedbackPanel.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled     = false;

            
            AnswerPanel.Children.Clear();
            for (int i = 0; i < q.Options.Count; i++)
            {
                var btn = new Button
                {
                    Content = q.Options[i],
                    Style   = (Style)FindResource("AnswerBtn"),
                    Margin  = new Thickness(0, 0, 0, 8),
                    Tag     = i
                };
                btn.Click += AnswerButton_Click;
                AnswerPanel.Children.Add(btn);
            }

            StatusBar.Text = "● Select an answer to continue";
        }

        // Fires when user clicks an answer button
        // Colours all buttons - green for correct, red for wrong selection
        // Shows explanation panel and unlocks the Next button
        // Updates score in the header badge
        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_answered) return;
            _answered = true;

            Button clicked      = (Button)sender;
            int selectedIndex   = (int)clicked.Tag;
            QuizQuestion q      = _questions[_currentIndex];
            bool isCorrect      = selectedIndex == q.CorrectIndex;

            if (isCorrect) _score++;

            
            foreach (var child in AnswerPanel.Children)
            {
                if (child is Button btn)
                {
                    int idx = (int)btn.Tag;
                    if (idx == q.CorrectIndex)
                    {
                        btn.Background  = new SolidColorBrush(Color.FromRgb(20, 80, 30));
                        btn.BorderBrush = new SolidColorBrush(CorrectColor);
                        btn.Foreground  = new SolidColorBrush(CorrectColor);
                    }
                    else if (idx == selectedIndex)
                    {
                        btn.Background  = new SolidColorBrush(Color.FromRgb(80, 20, 20));
                        btn.BorderBrush = new SolidColorBrush(WrongColor);
                        btn.Foreground  = new SolidColorBrush(WrongColor);
                    }
                    btn.IsEnabled = false;
                }
            }

            
            FeedbackPanel.Visibility   = Visibility.Visible;
            FeedbackPanel.Background   = isCorrect
                ? new SolidColorBrush(Color.FromRgb(15, 40, 15))
                : new SolidColorBrush(Color.FromRgb(40, 15, 15));
            FeedbackPanel.BorderBrush  = isCorrect
                ? new SolidColorBrush(CorrectColor)
                : new SolidColorBrush(WrongColor);
            FeedbackPanel.BorderThickness = new Thickness(1);

            FeedbackTitle.Text       = isCorrect ? "✔  Correct!" : "✖  Incorrect";
            FeedbackTitle.Foreground = isCorrect
                ? new SolidColorBrush(CorrectColor)
                : new SolidColorBrush(WrongColor);
            FeedbackText.Text        = q.Explanation;

            ScoreLabel.Text  = string.Format("{0} / {1}", _score, _currentIndex + 1);
            NextButton.IsEnabled = true;

            bool isLast = _currentIndex == _questions.Count - 1;
            NextButton.Content   = isLast ? "See My Score ›" : "Next Question ›";
            StatusBar.Text       = isCorrect
                ? "● Great answer! Click Next to continue."
                : "● Not quite — read the explanation and click Next.";
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _currentIndex++;
            ShowQuestion();
        }

        // Displays final results when all questions are answered
        // Score percentage determines which feedback message is shown
        // Play Again button reshuffles questions and restarts from Q1
        private void ShowFinalScore()
        {
            int total      = _questions.Count;
            double percent = (double)_score / total * 100;

           
            string feedback;
            if (percent >= 90)
                feedback = "Outstanding! You are a true cybersecurity pro!";
            else if (percent >= 70)
                feedback = "Great job! You have a solid understanding of cybersecurity.";
            else if (percent >= 50)
                feedback = "Good effort! Keep learning to stay safe online.";
            else
                feedback = "Keep learning to stay safe online! Review the topics and try again.";

            
            AnswerPanel.Children.Clear();
            FeedbackPanel.Visibility = Visibility.Collapsed;
            QuestionTypeBadge.Visibility = Visibility.Collapsed;

            QuestionText.Text = string.Format(
                "Quiz Complete!\n\nYou scored  {0} out of {1}  ({2}%)\n\n{3}",
                _score, total, (int)percent, feedback);
            QuestionText.FontSize = 15;

            ScoreLabel.Text      = string.Format("{0} / {1}", _score, total);
            ProgressFill.Width   = 760;
            ProgressLabel.Text   = "Complete!";
            NextButton.Content   = "Play Again ›";
            NextButton.IsEnabled = true;
            NextButton.Click    -= NextButton_Click;
            NextButton.Click    += (s, e) => RestartQuiz();
            StatusBar.Text       = string.Format("● Final score: {0}/{1} — {2}", _score, total, feedback);
        }

    
        private void RestartQuiz()
        {
            _currentIndex = 0;
            _score        = 0;
            _answered     = false;
            QuestionTypeBadge.Visibility = Visibility.Visible;
            QuestionText.FontSize        = 14;
            NextButton.Click            -= (s, e) => RestartQuiz();
            NextButton.Click            += NextButton_Click;
            BuildQuestions();
            ShowQuestion();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}
