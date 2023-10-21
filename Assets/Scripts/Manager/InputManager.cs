using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace BigDream
{
    public class InputManager : MonoSingleton<InputManager>
    {
        public override void Init()
        {

            // 键盘输入都写到这个里面
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                Debug.Log("f1");
            }
            else if (Input.GetKeyUp(KeyCode.A)) 
            {
                Debug.Log("A");
            }
        }
    }
}