using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrelloExportToObsidianKanbanBoard
{
    public class CardSimplified
    {
        public string? CardId { get; set; }
        public string? CardName { get; set; }
        public string? ListName { get; set; }
        public DateTime Date { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? Description { get; set; }
        public bool? CardClosed { get; set; }
    }
}
