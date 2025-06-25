namespace BullBrukBruker
{
    public class LevelProgressDataModel : DataModel<LevelProgressDTO>
    {
        public LevelProgressDataModel()
        {
            dataKey = DataKeys.LevelProgressKey;
            LoadData();
        }

        protected override LevelProgressDTO CreateDefaultData()
        {
            return new LevelProgressDTO
            {
                CurrentLevel = 1,
                HighestLevel = 1,
                StarsPerLevel = new(),
            };
        }
    }
}