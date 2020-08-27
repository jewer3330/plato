/* !!auto gen do not change
 
 */

using UnityEngine;
using System.Collections;

namespace Table
{
    public class TableLoad
    {
/*
        public static void LoadFromMemory()
        {
            battletarget.LoadFromMemory();
            effect.LoadFromMemory();
            monster.LoadFromMemory();
            player.LoadFromMemory();
            skill.LoadFromMemory();
            spawnmonster.LoadFromMemory();
            spawnplayer.LoadFromMemory();

        }
*/
        public static void LoadFromResources()
        {
            battletarget.LoadFromResources();
            effect.LoadFromResources();
            monster.LoadFromResources();
            player.LoadFromResources();
            skill.LoadFromResources();
            spawnmonster.LoadFromResources();
            spawnplayer.LoadFromResources();

        }

        public static void LoadBinFromResources()
        {
            battletarget.LoadBinFromResources();
            effect.LoadBinFromResources();
            monster.LoadBinFromResources();
            player.LoadBinFromResources();
            skill.LoadBinFromResources();
            spawnmonster.LoadBinFromResources();
            spawnplayer.LoadBinFromResources();

        }
/*
        public static void LoadFromStreaming()
        {
            battletarget.LoadFromStreaming();
            effect.LoadFromStreaming();
            monster.LoadFromStreaming();
            player.LoadFromStreaming();
            skill.LoadFromStreaming();
            spawnmonster.LoadFromStreaming();
            spawnplayer.LoadFromStreaming();

        }
*/
        public static void Clear()
        {
            battletarget.Clear();
            effect.Clear();
            monster.Clear();
            player.Clear();
            skill.Clear();
            spawnmonster.Clear();
            spawnplayer.Clear();

        }
		
        public static void UnLoad()
        {
            battletarget.UnLoad();
            effect.UnLoad();
            monster.UnLoad();
            player.UnLoad();
            skill.UnLoad();
            spawnmonster.UnLoad();
            spawnplayer.UnLoad();

        }
    }
}