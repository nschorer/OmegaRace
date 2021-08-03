using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    public enum AUDIO_EVENT
    {
        EXPLOSION,
        FENCE_HIT,
        MISSILE_HIT,
        PLAYER_KILLED,
        MINE_LAYED,
        MINE_DESPAWN,
        MISSILE_FIRE,
        MINE_ARMED
    }

    public class AudioManager 
    {
        private static AudioManager instance = null;
        public static AudioManager Instance()
        {
            if (instance == null)
            {
                instance = new AudioManager();
            }
            return instance;
        }

        IrrKlang.ISoundEngine sndEngine;


        private AudioManager()
        {
            sndEngine = new IrrKlang.ISoundEngine();
            
        }


        public static void PlaySoundEvent(AUDIO_EVENT Event_Type)
        {
            AudioManager inst = Instance();

            switch (Event_Type)
            {
                case AUDIO_EVENT.MISSILE_HIT:
                    inst.sndEngine.Play2D("laser_hit.wav");
                    break;
                case AUDIO_EVENT.FENCE_HIT:
                    inst.sndEngine.Play2D("fence_hit.wav");
                    break;
                case AUDIO_EVENT.PLAYER_KILLED:
                    inst.sndEngine.Play2D("ship_pop.wav");
                    break;
                case AUDIO_EVENT.MISSILE_FIRE:
                    inst.sndEngine.Play2D("firing.wav");
                    break;
                case AUDIO_EVENT.MINE_LAYED:
                    inst.sndEngine.Play2D("mine_lay.wav");
                    break;
                case AUDIO_EVENT.MINE_DESPAWN:
                    inst.sndEngine.Play2D("mine_pop.wav");
                    break;
                case AUDIO_EVENT.MINE_ARMED:
                    inst.sndEngine.Play2D("mine_arm.wav");
                    break;
            }
        }
    }
}
