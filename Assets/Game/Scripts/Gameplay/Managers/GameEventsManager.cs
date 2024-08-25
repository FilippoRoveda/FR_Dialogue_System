using System.Collections;

namespace Game
{
    public static class GameEventsManager
    {
        public static IEnumerator ExectuteEvent(string eventContent)
        {
            return EventExecutionAction(eventContent);
        }
        private static IEnumerator EventExecutionAction(string eventContent)
        {
            if (eventContent == "EndDemo")
            {
                EndDemo();
            }
            yield return null;
        }

        private static void EndDemo()
        {
            OptionsScreenController.Instance.CompleteDemo();
        }
    }
}