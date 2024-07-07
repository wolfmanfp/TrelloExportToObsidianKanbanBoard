namespace TrelloExportToObsidianKanbanBoard
{
    public static class BoardParser
    {
        public static TrelloBoardSimplified Build(TrelloBoard board)
        {
            var result = new TrelloBoardSimplified();
            result.Name = board.Name;
            result.FileName = string.Join("_", board.Name.Split(Path.GetInvalidFileNameChars()));
            result.Lists = new List<TrelloListSimplified>();

            var fuckingLists = board.Lists.Where(x => !x.Closed).ToList();
            foreach (var item in fuckingLists)
            {
                var list = new TrelloListSimplified();
                list.Name = item.Name;
                list.Tasks = new List<string>();
                result.Lists.Add(list);
            }

            var cards = board.Actions
                .Where(x => x.Type != ActionType.DeleteCard)
                .Where(x => x.Type != ActionType.AddChecklistToCard)
                .Select(x => new CardSimplified
                {
                    CardId = x.Data?.Card?.Id,
                    CardName = x.Data?.Card?.Name,
                    ListName = x.Data?.List?.Name ?? x.Data?.ListAfter?.Name,
                    Date = x.Date,
                    AttachmentUrl = x.Data?.Attachment?.Url,
                    Description = x.Data?.Card?.Desc,
                    CardClosed = x.Data?.Card?.Closed,
                });
            var filteredCards = cards
                .GroupBy(x => x.CardId) // Group by card ID
                .Select(group => group.OrderByDescending(x => x.Date).First()) // Select the latest entry based on date
                .Where(x => x.CardClosed == false)
                .ToList();

            foreach (var item in filteredCards)
            {
                var list = result.Lists.FirstOrDefault(x => x.Name == item.ListName);
                if (list == null)
                {
                    continue;
                }
                var task = list.Tasks.FirstOrDefault(x => x == item.CardName);
                if (task == null)
                {
                    list.Tasks.Add(item.CardName);
                }
            }

            return result;
        }

        public static string GenerateMarkdown(TrelloBoardSimplified board)
        {
            var markdown = new List<string>();

            // Add front matter
            markdown.Add("---");
            markdown.Add("kanban-plugin: basic");
            markdown.Add("---");
            markdown.Add("");

            // Generate markdown for each list and tasks
            foreach (var list in board.Lists)
            {
                markdown.Add($"## {list.Name}");
                markdown.Add("");

                foreach (var task in list.Tasks)
                {
                    markdown.Add($"- [ ] {task}");
                }

                markdown.Add("");
            }

            // Add kanban settings
            markdown.Add("%% kanban:settings");
            markdown.Add("{\"kanban-plugin\":\"basic\"}");
            markdown.Add("%%");

            // Combine lines into a single string
            string markdownContent = string.Join(Environment.NewLine, markdown);

            return markdownContent;
        }
    }

}
