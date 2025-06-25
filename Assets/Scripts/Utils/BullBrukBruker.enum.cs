using System;

namespace BullBrukBruker
{
    public enum BlockID
    {
        None,
        BlockBlack,
        BlockBlue,
        BlockGrey,
        BlockGreen,
        BlockOrange,
        BlockPink,
        BlockRed,
        BlockYellow,
    }

    public enum ObjectID
    {
        None = 0,
        Ball,
        Paddle,
        Item,
        HorizontalLevelBound,
        VerticalLevelBound,
        DeathBound,
    }

    public enum ItemID
    {
        None = 0,
        SpreadItem,
        TriplicateItem,
    }

    public enum PaddleStateID
    {
        None = 0,
        Ideling,
        Catching,
        Aiming,
    }

    public enum GameStateID
    {
        None = 0,
        MainMenu,
        SelectLevel,
        Load,
        Play,
        Pause,
        Win,
        Over
    }

    public enum EventID
    {
        None = 0,
        SelectLevelButton_Clicked,
        PlayButton_Clicked,
        LevelButton_Clicked,
        ReturnMenuButton_Clicked,
        PauseGameButton_Clicked,
        ContinueButton_Clicked,
        OutOfBalls,
        OutOfCells,
        OutOfLevels,
        ReplayButton_Clicked,
        HomeButton_Clicked,
        NextLevelButton_Clicked,
        LevelProgressData_Changed,
    }

    public enum DataID
    {
        None,
        LevelProgress
    }
}