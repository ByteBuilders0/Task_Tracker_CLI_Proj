using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Task_Tracker_CLI_Proj.Models;

namespace Task_Tracker_CLI_Proj.Services
{
    public class TaskEntityService
    {
        private readonly string taskDataFilePath = "task.json";
        private List<TaskEntities> taskList;
        private readonly JsonSerializerOptions jsonOptions;

        public TaskEntityService()
        {
            jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() } 
            };
            LoadTasks();
        }

        private void LoadTasks()
        {
            if (File.Exists(taskDataFilePath))
            {
                string json = File.ReadAllText(taskDataFilePath);
                try
                {
                    taskList = JsonSerializer.Deserialize<List<TaskEntities>>(json, jsonOptions) ?? new List<TaskEntities>();
                }
                catch (JsonException)
                {
                    Console.WriteLine("Warning: Could not parse task.json. Is it formatted correctly with string statuses (e.g., \"todo\")?");
                    taskList = new List<TaskEntities>(); 
                }
            }
            else
            {
                taskList = new List<TaskEntities>();
            }
        }

        public void SaveTasks()
        {
            string json = JsonSerializer.Serialize(taskList, jsonOptions);
            File.WriteAllText(taskDataFilePath, json, Encoding.UTF8);
        }

        public void AddTask(string name, string taskDescription)
        {
            var taskItem = new TaskEntities
            {
                Task_ID = taskList.Any() ? taskList.Max(t => t.Task_ID) + 1 : 1,
                Task_Name = name,
                Task_Description = taskDescription,
                Task_Status = TaskObservationStatus.todo,
                Task_Created_At = DateTime.UtcNow,
                Task_Updated_At = DateTime.UtcNow
            };
            taskList.Add(taskItem);
            Console.WriteLine($"Task '{taskItem.Task_ID}' added successfully.");
        }

        public void UpdateTask(int taskId, string name, string taskDescription)
        {
            var task = taskList.Find(t => t.Task_ID == taskId);
            if (task == null) { Console.WriteLine($"Task with ID {taskId} not found."); return; }

            task.Task_Name = name;
            task.Task_Description = taskDescription;
            task.Task_Updated_At = DateTime.UtcNow;
            Console.WriteLine($"Task {taskId} updated successfully.");
        }

        public void DeleteTask(int id)
        {
            var task = taskList.Find(t => t.Task_ID == id);
            if (task == null) { Console.WriteLine($"Task with ID {id} not found."); return; }

            taskList.Remove(task);
            Console.WriteLine($"Task {id} deleted successfully.");
        }

        public void MarkTaskInProgress(int id)
        {
            UpdateTaskStatus(id, TaskObservationStatus.in_progress);
        }

        public void MarkTaskDone(int id)
        {
            UpdateTaskStatus(id, TaskObservationStatus.done);
        }

        private void UpdateTaskStatus(int id, TaskObservationStatus status)
        {
            var task = taskList.Find(t => t.Task_ID == id);
            if (task == null) { Console.WriteLine($"Task with ID {id} not found."); return; }

            task.Task_Status = status;
            task.Task_Updated_At = DateTime.UtcNow;
            Console.WriteLine($"Task {id} marked as {status}.");
        }

        public void ListTasks()
        {
            if (!taskList.Any()) { Console.WriteLine("No tasks found."); return; }
            foreach (var task in taskList)
            {
                Console.WriteLine($"[{task.Task_ID}] {task.Task_Name} {task.Task_Description} [{task.Task_Status}]");
            }
        }

        public void ListTasksByStatus(string status)
        {
            if (!Enum.TryParse<TaskObservationStatus>(status, true, out var filterStatus))
            {
                Console.WriteLine("Invalid status. Use 'todo', 'in_progress', or 'done'.");
                return;
            }

            var filtered = taskList.Where(t => t.Task_Status == filterStatus).ToList();
            if (!filtered.Any()) { Console.WriteLine($"No tasks with status '{status}'."); return; }

            foreach (var task in filtered)
            {
                Console.WriteLine($"[{task.Task_ID}] {task.Task_Name} {task.Task_Description} [{task.Task_Status}]");
            }
        }

        public static void DisplayMenu()
        {
            Console.WriteLine("\nTask Tracker CLI");
            Console.WriteLine("Usage: dotnet run [command] [options]");
            Console.WriteLine("Commands:");
            Console.WriteLine("  add \"<name>\" \"<description>\"    - Add a new task");
            Console.WriteLine("  update <id> \"<name>\" \"<desc>\"   - Update a task");
            Console.WriteLine("  delete <id>                - Delete a task");
            Console.WriteLine("  mark-in-progress <id>      - Mark task as in-progress");
            Console.WriteLine("  mark-done <id>             - Mark task as done");
            Console.WriteLine("  list [status]              - List tasks (all, todo, in_progress, done)");
            Console.WriteLine("  display                    - Show this menu");
        }
    }
}
