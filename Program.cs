// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Task_Tracker_CLI_Proj.Models;
using Task_Tracker_CLI_Proj.Services;

public class Program
{
    //private static readonly string taskDataFilePath = "task.json";
    //private static List<TaskEntities> taskList = new List<TaskEntities>();

    public static void Main(string[] args)
    {
        var taskentityService = new TaskEntityService();

        // TaskEntityService.LoadTasks();
        TaskEntityService.DisplayMenu();

        if (args.Length == 0)
        {
            TaskEntityService.DisplayMenu();
            return;
        }

        string command = args[0].ToLower();

        try
        {
            switch (command)
            {

                case "add":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: add <task_name> <task_description>");
                        return;
                    }
                    string taskName = args[1];
                    string description = args[2];// string.Join(" ", args.Skip(1)); // string.Join(" ", args, 2, args.Length - 1);
                    taskentityService.AddTask(taskName, description);
                    break;

                case "list":
                    if (args.Length == 1)
                    {
                        taskentityService.ListTasks();
                    }
                    else
                    {
                        string taskStatus = args[1].ToLower();
                        taskentityService.ListTasksByStatus(taskStatus);
                    }
                    taskentityService.ListTasks();
                    break;

                case "update":
                    if (args.Length < 4)
                    {
                        Console.WriteLine("Usage: update <task_id> <new_task_name> <new_task_description>");
                        return;
                    }
                    if (!int.TryParse(args[1], out int taskUpdatedId))
                    {
                        Console.WriteLine("Invalid task ID. Please provide a valid integer.");
                        return;
                    }
                    string newTaskName = args[2];
                    string taskUpdateDescription = args[3]; // string.Join(" ", args.Skip(2)); // string.Join(" ", args, 3, args.Length - 1);
                    taskentityService.UpdateTask(taskUpdatedId, newTaskName, taskUpdateDescription);
                    break;

                case "delete":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: delete <task_id>");
                        return;
                    }
                    if (!int.TryParse(args[1], out int taskIdToDelete))
                    {
                        Console.WriteLine("Invalid task ID. Please provide a valid integer.");
                        return;
                    }
                    taskentityService.DeleteTask(taskIdToDelete);
                    break;

                case "mark-in-progress":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: mark-in-progress <task_id>");
                        return;
                    }
                    if (!int.TryParse(args[1], out int taskIdInProgress))
                    {
                        Console.WriteLine("Invalid task ID. Please provide a valid integer.");
                        return;
                    }
                    taskentityService.MarkTaskInProgress(taskIdInProgress);
                    break;

                case "mark-done":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: mark-done <task_id>");
                        return;
                    }
                    if (!int.TryParse(args[1], out int taskIdDone))
                    {
                        Console.WriteLine("Invalid task ID. Please provide a valid integer.");
                        return;
                    }
                    taskentityService.MarkTaskDone(taskIdDone);
                break;

                case "dispaly":
                    TaskEntityService.DisplayMenu();
                    break;

                case "exit":
                    Console.WriteLine("Exiting the application.");
                    return;

                default:
                    Console.WriteLine("Unknown command. Use 'help' to see available commands.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            taskentityService.SaveTasks();
        }
    }

}
