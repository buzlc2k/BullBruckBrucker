namespace BullBrukBruker{
    public class StarImagesContainerInButtonController : StarImagesContainerController
    {
        public override void ActiveStars(int num)
        {
            if (num == 0) return;

            foreach (var star in starImages)
                star.gameObject.SetActive(true);

            base.ActiveStars(num);
        }
    }
}