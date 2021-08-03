using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    public class KeyState
    {
        Azul.AZUL_KEY key;

        public bool prevState;
        public bool curState;

        public KeyState(Azul.AZUL_KEY _key)
        {
            key = _key;
        }

        public bool Pressed()
        {
            return curState;
        }

        public bool PressedDown()
        {
            return ((prevState == false) && (curState == true));
        }

        public bool PressedUp()
        {
            return ((prevState == true) && (curState == false));
        }

        public void Update()
        {
            prevState = curState;
            curState = Azul.Input.GetKeyState(key);
        }
    }

    public class MouseButtonState
    {
        Azul.AZUL_MOUSE key;

        public bool prevState;
        public bool curState;

        public MouseButtonState(Azul.AZUL_MOUSE _key)
        {
            key = _key;
        }

        public bool Pressed()
        {
            return curState;
        }

        public bool PressedDown()
        {
            return ((prevState == false) && (curState == true));
        }

        public bool PressedUp()
        {
            return ((prevState == true) && (curState == false));
        }

        public void Update()
        {
            prevState = curState;
            curState = Azul.Input.GetKeyState(key);
        }
    }
}
