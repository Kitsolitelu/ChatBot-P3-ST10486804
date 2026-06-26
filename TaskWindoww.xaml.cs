using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace ChatBotP2
{
    public partial class TaskWindoww : Window
    {
       
        private static readonly Dictionary<string, string> _taskDescriptions =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["review privacy settings"] = "Review account privacy settings to ensure your data is protected.",
                ["enable 2fa"] = "Set up two-factor authentication on all important accounts.",
                ["update passwords"] = "Update passwords for all accounts and ensure they are unique and strong.",
                ["install antivirus"] = "Install and configure antivirus software on your device.",
                ["backup data"] = "Create an offline backup of your important files and documents.",
                ["check app permissions"] = "Review which apps have access to your camera, microphone and location.",
                ["update software"] = "Update your operating system and all installed applications.",
                ["set up vpn"] = "Configure a VPN for use on public Wi-Fi networks."
            };

        public TaskWindoww()
        {
            InitializeComponent();
            
            DatabaseHelper.Initialise();
            LoadTasks();
        }

       
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleInput.Text.Trim();
            string desc = DescInput.Text.Trim();
            string reminder = ReminderInput.Text.Trim();

            
            if (string.IsNullOrWhiteSpace(title))
            {
                SetStatus("Please enter a task title.", isError: true);
                TitleInput.Focus();
                return;
            }

            
            if (string.IsNullOrWhiteSpace(desc))
            {
                if (_taskDescriptions.TryGetValue(title.ToLower(), out string autoDesc))
                    desc = autoDesc;
                else
                    desc = "Complete the task: " + title;
            }

           
            DatabaseHelper.AddTask(title, desc, reminder);

            
            TitleInput.Clear();
            DescInput.Clear();
            ReminderInput.Clear();

           
            LoadTasks();

            string reminderMsg = string.IsNullOrEmpty(reminder)
                ? ""
                : $" I'll remind you: {reminder}.";

            SetStatus($"Task added: \"{title}\".{reminderMsg}", isError: false);
            TitleInput.Focus();
        }

        private void LoadTasks()
        {
            TaskListPanel.Children.Clear();

            List<CyberTask> tasks = DatabaseHelper.GetAllTasks();

            if (tasks.Count == 0)
            {
                TaskListPanel.Children.Add(new TextBlock
                {
                    Text = "No tasks yet. Add your first cybersecurity task above!",
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Color.FromRgb(100, 116, 139)),
                    Margin = new Thickness(0, 20, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center
                });
                TaskCount.Text = "0 tasks";
                return;
            }

            int complete = 0;
            int incomplete = 0;

            foreach (CyberTask task in tasks)
            {
                if (task.IsComplete) complete++;
                else incomplete++;

                TaskListPanel.Children.Add(BuildTaskCard(task));
            }

            TaskCount.Text = $"{tasks.Count} tasks  |  {complete} done  |  {incomplete} pending";
        }

       
        private Border BuildTaskCard(CyberTask task)
        {
            
            Color borderColor = task.IsComplete
                ? Color.FromRgb(34, 197, 94)
                : Color.FromRgb(110, 64, 201);

            Color bgColor = task.IsComplete
                ? Color.FromRgb(15, 25, 15)
                : Color.FromRgb(15, 11, 30);

            var card = new Border
            {
                Background = new SolidColorBrush(bgColor),
                BorderBrush = new SolidColorBrush(borderColor),
                BorderThickness = new Thickness(2, 0, 0, 0),
                CornerRadius = new CornerRadius(0, 6, 6, 0),
                Padding = new Thickness(14, 10, 14, 10),
                Margin = new Thickness(0, 0, 0, 8)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

           
            var info = new StackPanel();

           
            var titleRow = new StackPanel { Orientation = Orientation.Horizontal };

            if (task.IsComplete)
            {
                titleRow.Children.Add(new TextBlock
                {
                    Text = "✔ ",
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Color.FromRgb(34, 197, 94)),
                    VerticalAlignment = VerticalAlignment.Center
                });
            }

            titleRow.Children.Add(new TextBlock
            {
                Text = task.Title,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 13,
                FontWeight = FontWeights.Bold,
                Foreground = task.IsComplete
                                    ? new SolidColorBrush(Color.FromRgb(100, 116, 139))
                                    : new SolidColorBrush(Color.FromRgb(196, 181, 253)),
                TextDecorations = task.IsComplete ? TextDecorations.Strikethrough : null
            });
            info.Children.Add(titleRow);

           
            info.Children.Add(new TextBlock
            {
                Text = task.Description,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(148, 163, 184)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 4, 0, 0)
            });

            
            var metaRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 6, 0, 0)
            };

            if (!string.IsNullOrEmpty(task.Reminder))
            {
                metaRow.Children.Add(new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(14, 116, 144)),
                    CornerRadius = new CornerRadius(4),
                    Padding = new Thickness(8, 2, 8, 2),
                    Margin = new Thickness(0, 0, 8, 0),
                    Child = new TextBlock
                    {
                        Text = "⏰ " + task.Reminder,
                        FontFamily = new FontFamily("Consolas"),
                        FontSize = 10,
                        Foreground = Brushes.White
                    }
                });
            }

            metaRow.Children.Add(new TextBlock
            {
                Text = "Added: " + task.CreatedAt,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 10,
                Foreground = new SolidColorBrush(Color.FromRgb(71, 85, 105)),
                VerticalAlignment = VerticalAlignment.Center
            });

            info.Children.Add(metaRow);
            Grid.SetColumn(info, 0);
            grid.Children.Add(info);

            
            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(12, 0, 0, 0)
            };

            if (!task.IsComplete)
            {
                var completeBtn = new Button
                {
                    Content = "✔ Done",
                    Style = (Style)FindResource("CompleteBtn"),
                    Margin = new Thickness(0, 0, 6, 0),
                    Tag = task.Id
                };
                completeBtn.Click += CompleteTask_Click;
                btnPanel.Children.Add(completeBtn);
            }

            var deleteBtn = new Button
            {
                Content = "✖ Delete",
                Style = (Style)FindResource("DeleteBtn"),
                Tag = task.Id
            };
            deleteBtn.Click += DeleteTask_Click;
            btnPanel.Children.Add(deleteBtn);

            Grid.SetColumn(btnPanel, 1);
            grid.Children.Add(btnPanel);

            card.Child = grid;
            return card;
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                DatabaseHelper.MarkComplete(id);
                LoadTasks();
                SetStatus("Task marked as complete!", isError: false);
            }
        }

      
       
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to delete this task?",
                    "Delete Task",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    DatabaseHelper.DeleteTask(id);
                    LoadTasks();
                    SetStatus("Task deleted.", isError: false);
                }
            }
        }

     
        private void BackButton_Click(object sender, RoutedEventArgs e) => Close();

      
        private void SetStatus(string msg, bool isError)
        {
            StatusBar.Text = "● " + msg;
            StatusBar.Foreground = isError
                ? new SolidColorBrush(Color.FromRgb(239, 68, 68))
                : new SolidColorBrush(Color.FromRgb(74, 222, 128));
        }
    }
}