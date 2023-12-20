using Azure;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MathWars.Models;

public class TasksAndCategories
{
	public int TaskId { get; set; }
	public Tasks Task { get; set; } = null!;

	public int TaskCategoryId { get; set; }
	public TasksCategory TaskCategory { get; set; } = null!;
}
