using System;
using System.Collections.Generic;

#nullable disable

namespace H5ServersideProgrammering.Areas.TodoList.Models
{
    public partial class ToDoList
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
    }
}
