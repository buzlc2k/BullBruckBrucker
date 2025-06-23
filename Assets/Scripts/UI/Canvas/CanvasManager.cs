using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BullBrukBruker{
    public class CanvasManager : SingletonMono<CanvasManager>
    {
        private BaseCanvasController currentCanvas;

        [SerializeField] private List<BaseCanvasController> canvases = new();

        public IEnumerator InitCanvasManager()
        {
            GetCanvases();

            foreach (var canvas in canvases)
                yield return StartCoroutine(canvas.InitCanvas());

            yield return null;
        }
        private void GetCanvases(){
            if (canvases == null || canvases.Count == 0)
            {
                canvases.AddRange(GetComponentsInChildren<BaseCanvasController>());
            }
        }

        public IEnumerator SetActiveCanvas(GameStateID currentGameState){
            if(currentCanvas != null) 
                yield return StartCoroutine(currentCanvas.TurnOffCanvas());

            foreach(var canvas in canvases){
                if(!currentGameState.Equals(canvas.RespondingState)) continue;
                currentCanvas = canvas;
                yield return StartCoroutine(canvas.TurnOnCanvas());
            }
        }
    }
}