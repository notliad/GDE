using Assets.Scripts.Consts;
using Photon.Pun.UtilityScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Extensions
{
    public static class PhotonPlayerExtensions
    {
        public static bool JoinTeam(this Photon.Realtime.Player player, int team)
        {
            var currentTeam = player.GetPhotonTeam();
            if (currentTeam == null)
                player.JoinTeam(Teams.GetTeam(team));
            else
                player.SwitchTeam(Teams.GetTeam(team));
            return true;
        }
    }
}
