using Firebase.Database;

namespace BullBrukBruker
{
    public class LevelProgressDataModel : DataModel<LevelProgressDTO>
    {
        public LevelProgressDataModel(string userID)
        {
            dataNode = DataNodes.LevelProgressKey;
            this.userID = userID;
            dbRef = FirebaseDatabase.DefaultInstance.RootReference.Child(dataNode).Child(userID);
        }

        protected override LevelProgressDTO CreateDefaultData()
        {
            return new LevelProgressDTO
            {
                CurrentLevel = 1,
                HighestLevel = 1,
                StarsPerLevel = new(){0},
            };
        }
    }
}