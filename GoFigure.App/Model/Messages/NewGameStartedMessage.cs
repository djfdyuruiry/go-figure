using System.Collections.Generic;

namespace GoFigure.App.Model.Messages
{
    class NewGameStartedMessage
    {
        public int Target { get; set; }

        public IEnumerable<int> AvailableNumbers { get; set; }
    }
}
