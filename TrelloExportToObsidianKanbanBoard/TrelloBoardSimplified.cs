namespace TrelloExportToObsidianKanbanBoard
{
    public class TrelloBoardSimplified
    {
        public string Name { get; set; }
        public List<TrelloListSimplified> Lists { get; set; }
    }

    public class TrelloListSimplified
    {
        public string Name { get; set; }
        public List<string> Tasks { get; set; }
    }

}
