public static class Events
{
    public class PauseGame
    {
    }

    public class ResumeGame
    {
    }

    public class SwitchMainCharacter
    {
        public int NewIndex;
        public int OldIndex;
    }

    public class CharacterStateChanged
    {
        public int CharacterIndex;
    }

    // 战斗通知

    // 挂上物理破防
    public class OnPhysicalDefenseBreakApplied
    {
    }

    //当有人释放了连携技
    public class OnLinkSkillTriggered
    {
    }

    //当破防层数被消耗了
    public class OnPhysicalDefenseBreakConsumed
    {
        public int breakStack;
    }

    public class OnLinkSkillQueueChanged
    {
    }
}
