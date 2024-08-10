using System;
using System.Collections;

public static class GameEventsManager
{

    public static IEnumerator ExectuteEvent(string eventContent)
    {
        return EventExecutionAction(eventContent);
    }
    private static IEnumerator EventExecutionAction(string eventContent)
    {
        if (eventContent == "EndPhase_01")
        {
            EndPhase_01();
        }
        else if (eventContent == "EndPhase_02") 
        {
            EndPhase_02();
        }
        else if (eventContent == "EndDemo") 
        {
            EndDemo();
        }
        yield return null;
    }

    private static void EndDemo()
    {
        throw new NotImplementedException();
    }

    private static void EndPhase_02()
    {
        throw new NotImplementedException();
    }

    private static void EndPhase_01()
    {
        throw new NotImplementedException();
    }
}
