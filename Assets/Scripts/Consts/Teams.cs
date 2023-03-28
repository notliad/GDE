using Photon.Pun.UtilityScripts;

namespace Assets.Scripts.Consts
{
    public class Teams
    {
        public static PhotonTeam Team_1 { get; private set; } = new PhotonTeam { Code = (byte)1, Name = "Team 1" };
        public static PhotonTeam Team_2 { get; private set; } = new PhotonTeam { Code = (byte)2, Name = "Team 2" };

        public static PhotonTeam GetTeam(int team) => team == 1 ? Team_1 : Team_2;
    }
}
