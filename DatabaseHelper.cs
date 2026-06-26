using System;
using System.Collections.Generic;
using System.IO;

namespace ChatBotP2
{
    
    public class CyberTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public bool IsComplete { get; set; }
        public string CreatedAt { get; set; }
    }

   
    public class DatabaseHelper
    {
        private static readonly string FilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cyberbot_tasks.txt");

        
        public static void Initialise()
        {
            if (!File.Exists(FilePath))
                File.WriteAllText(FilePath, "");
        }

        
        public static int AddTask(string title, string description, string reminder)
        {
            List<CyberTask> tasks = GetAllTasks();

           
            int newId = tasks.Count == 0 ? 1 : tasks[tasks.Count - 1].Id + 1;

            string line = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                newId,
                Escape(title),
                Escape(description),
                Escape(reminder ?? ""),
                "0",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            File.AppendAllText(FilePath, line + Environment.NewLine);
            return newId;
        }

        
        public static List<CyberTask> GetAllTasks()
        {
            var tasks = new List<CyberTask>();

            if (!File.Exists(FilePath))
                return tasks;

            string[] lines = File.ReadAllLines(FilePath);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split('|');
                if (parts.Length < 6) continue;

                tasks.Add(new CyberTask
                {
                    Id = int.Parse(parts[0]),
                    Title = Unescape(parts[1]),
                    Description = Unescape(parts[2]),
                    Reminder = Unescape(parts[3]),
                    IsComplete = parts[4] == "1",
                    CreatedAt = parts[5]
                });
            }

            
            tasks.Reverse();
            return tasks;
        }

       
        public static void MarkComplete(int id)
        {
            UpdateTask(id, isComplete: true);
        }

        
        public static void DeleteTask(int id)
        {
            if (!File.Exists(FilePath)) return;

            string[] lines = File.ReadAllLines(FilePath);
            var kept = new List<string>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split('|');
                if (parts.Length < 1) continue;
                if (parts[0] != id.ToString())
                    kept.Add(line);
            }

            File.WriteAllLines(FilePath, kept);
        }

       
        private static void UpdateTask(int id, bool isComplete)
        {
            if (!File.Exists(FilePath)) return;

            string[] lines = File.ReadAllLines(FilePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split('|');
                if (parts.Length < 6) continue;

                if (parts[0] == id.ToString())
                {
                    parts[4] = isComplete ? "1" : "0";
                    lines[i] = string.Join("|", parts);
                }
            }

            File.WriteAllLines(FilePath, lines);
        }

        
        private static string Escape(string s)
        {
            return s == null ? "" : s.Replace("|", "[PIPE]").Replace("\n", " ");
        }

        private static string Unescape(string s)
        {
            return s == null ? "" : s.Replace("[PIPE]", "|");
        }
    }
}

