using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming_Examination_Platform.Model
{
  public static class GlobalReview
  {
    public static HashSet<int> tasks = new HashSet<int>();

    
    
    /// <summary>
    /// DEBUG
    /// </summary>
    ///
    public static void Clear()
    {
      tasks.Clear();
    }
    public static void PrintToConsole()
    {
      foreach (var task in tasks)
      {
        Console.WriteLine(task+" ");
      }
    }
  }
}