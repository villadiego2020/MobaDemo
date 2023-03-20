using UnityEngine;

namespace Assets.SimulateTest
{
    public class SimulateTest : MonoBehaviour
    {
        public SimulateTestManager manager;

        public int offsetX;
        public int offsetY;

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));

            if (GUILayout.Button("Add 10 Objects"))
            {
                //for (int i = 0; i < 10; i++)
                //{
                //    manager.Spawn();
                //}
            }
        }
    }
}