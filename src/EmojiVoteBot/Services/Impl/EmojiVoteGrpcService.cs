using Emojivoto.V1;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmojiVoteBot.Services.Impl
{
    public class EmojiVoteGrpcService : IEmojiVoteService
    {
        private readonly VotingService.VotingServiceClient _votingClient;
        private readonly EmojiService.EmojiServiceClient _emojiClient;

        public EmojiVoteGrpcService(VotingService.VotingServiceClient votingClient, EmojiService.EmojiServiceClient emojiClient)
        {
            _votingClient = votingClient;
            _emojiClient = emojiClient;
        }

        public async Task<IEnumerable<Emoji>> ListEmojis()
        {
            var response = await _emojiClient.ListAllAsync(new ListAllEmojiRequest());
            var result = response.List.Select(emoji => new Emoji { Shortcode = emoji.Shortcode, Unicode = emoji.Unicode }).ToList();
            return result.AsEnumerable();
        }

        public async Task<Emoji?> FindByShortCode(string shortcode)
        {
            var response = await _emojiClient.FindByShortcodeAsync(new FindByShortcodeRequest { Shortcode = shortcode });
            return response.Emoji != null ?
                new Emoji { Shortcode = response.Emoji.Shortcode, Unicode = response.Emoji.Unicode } :
                null;
        }

        public async Task<bool> Vote(string choice)
        {
            //Check shortcode exists
            var emojiresponse = await _emojiClient.FindByShortcodeAsync(new FindByShortcodeRequest { Shortcode = choice });
            if (emojiresponse.Emoji == null)
            {
                return false;
            }

            switch (choice)
            {
                case ":poop:":
                    _votingClient.VotePoop(new VoteRequest());
                    break;
                case ":joy:":
                    _votingClient.VoteJoy(new VoteRequest());
                    break;
                case ":sunglasses:":
                    _votingClient.VoteSunglasses(new VoteRequest());
                    break;
                case ":relaxed:":
                    _votingClient.VoteRelaxed(new VoteRequest());
                    break;
                case ":stuck_out_tongue_winking_eye:":
                    _votingClient.VoteStuckOutTongueWinkingEye(new VoteRequest());
                    break;
                case ":money_mouth_face:":
                    _votingClient.VoteMoneyMouthFace(new VoteRequest());
                    break;
                case ":flushed:":
                    _votingClient.VoteFlushed(new VoteRequest());
                    break;
                case ":mask:":
                    _votingClient.VoteMask(new VoteRequest());
                    break;
                case ":nerd_face:":
                    _votingClient.VoteNerdFace(new VoteRequest());
                    break;
                case ":ghost:":
                    _votingClient.VoteGhost(new VoteRequest());
                    break;
                case ":skull_and_crossbones:":
                    _votingClient.VoteSkullAndCrossbones(new VoteRequest());
                    break;
                case ":heart_eyes_cat:":
                    _votingClient.VoteHeartEyesCat(new VoteRequest());
                    break;
                case ":hear_no_evil:":
                    _votingClient.VoteHearNoEvil(new VoteRequest());
                    break;
                case ":see_no_evil:":
                    _votingClient.VoteSeeNoEvil(new VoteRequest());
                    break;
                case ":speak_no_evil:":
                    _votingClient.VoteSpeakNoEvil(new VoteRequest());
                    break;
                case ":boy:":
                    _votingClient.VoteBoy(new VoteRequest());
                    break;
                case ":girl:":
                    _votingClient.VoteGirl(new VoteRequest());
                    break;
                case ":man:":
                    _votingClient.VoteMan(new VoteRequest());
                    break;
                case ":woman:":
                    _votingClient.VoteWoman(new VoteRequest());
                    break;
                case ":older_man:":
                    _votingClient.VoteOlderMan(new VoteRequest());
                    break;
                case ":policeman:":
                    _votingClient.VotePoliceman(new VoteRequest());
                    break;
                case ":guardsman:":
                    _votingClient.VoteGuardsman(new VoteRequest());
                    break;
                case ":construction_worker_man:":
                    _votingClient.VoteConstructionWorkerMan(new VoteRequest());
                    break;
                case ":prince:":
                    _votingClient.VotePrince(new VoteRequest());
                    break;
                case ":princess:":
                    _votingClient.VotePrincess(new VoteRequest());
                    break;
                case ":man_in_tuxedo:":
                    _votingClient.VoteManInTuxedo(new VoteRequest());
                    break;
                case ":bride_with_veil:":
                    _votingClient.VoteBrideWithVeil(new VoteRequest());
                    break;
                case ":mrs_claus:":
                    _votingClient.VoteMrsClaus(new VoteRequest());
                    break;
                case ":santa:":
                    _votingClient.VoteSanta(new VoteRequest());
                    break;
                case ":turkey:":
                    _votingClient.VoteTurkey(new VoteRequest());
                    break;
                case ":rabbit:":
                    _votingClient.VoteRabbit(new VoteRequest());
                    break;
                case ":no_good_woman:":
                    _votingClient.VoteNoGoodWoman(new VoteRequest());
                    break;
                case ":ok_woman:":
                    _votingClient.VoteOkWoman(new VoteRequest());
                    break;
                case ":raising_hand_woman:":
                    _votingClient.VoteRaisingHandWoman(new VoteRequest());
                    break;
                case ":bowing_man:":
                    _votingClient.VoteBowingMan(new VoteRequest());
                    break;
                case ":man_facepalming:":
                    _votingClient.VoteManFacepalming(new VoteRequest());
                    break;
                case ":woman_shrugging:":
                    _votingClient.VoteWomanShrugging(new VoteRequest());
                    break;
                case ":massage_woman:":
                    _votingClient.VoteMassageWoman(new VoteRequest());
                    break;
                case ":walking_man:":
                    _votingClient.VoteWalkingMan(new VoteRequest());
                    break;
                case ":running_man:":
                    _votingClient.VoteRunningMan(new VoteRequest());
                    break;
                case ":dancer:":
                    _votingClient.VoteDancer(new VoteRequest());
                    break;
                case ":man_dancing:":
                    _votingClient.VoteManDancing(new VoteRequest());
                    break;
                case ":dancing_women:":
                    _votingClient.VoteDancingWomen(new VoteRequest());
                    break;
                case ":rainbow:":
                    _votingClient.VoteRainbow(new VoteRequest());
                    break;
                case ":skier:":
                    _votingClient.VoteSkier(new VoteRequest());
                    break;
                case ":golfing_man:":
                    _votingClient.VoteGolfingMan(new VoteRequest());
                    break;
                case ":surfing_man:":
                    _votingClient.VoteSurfingMan(new VoteRequest());
                    break;
                case ":basketball_man:":
                    _votingClient.VoteBasketballMan(new VoteRequest());
                    break;
                case ":biking_man:":
                    _votingClient.VoteBikingMan(new VoteRequest());
                    break;
                case ":point_up_2:":
                    _votingClient.VotePointUp2(new VoteRequest());
                    break;
                case ":vulcan_salute:":
                    _votingClient.VoteVulcanSalute(new VoteRequest());
                    break;
                case ":metal:":
                    _votingClient.VoteMetal(new VoteRequest());
                    break;
                case ":call_me_hand:":
                    _votingClient.VoteCallMeHand(new VoteRequest());
                    break;
                case ":thumbsup:":
                    _votingClient.VoteThumbsup(new VoteRequest());
                    break;
                case ":wave:":
                    _votingClient.VoteWave(new VoteRequest());
                    break;
                case ":clap:":
                    _votingClient.VoteClap(new VoteRequest());
                    break;
                case ":raised_hands:":
                    _votingClient.VoteRaisedHands(new VoteRequest());
                    break;
                case ":pray:":
                    _votingClient.VotePray(new VoteRequest());
                    break;
                case ":dog:":
                    _votingClient.VoteDog(new VoteRequest());
                    break;
                case ":cat2:":
                    _votingClient.VoteCat2(new VoteRequest());
                    break;
                case ":pig:":
                    _votingClient.VotePig(new VoteRequest());
                    break;
                case ":hatching_chick:":
                    _votingClient.VoteHatchingChick(new VoteRequest());
                    break;
                case ":snail:":
                    _votingClient.VoteSnail(new VoteRequest());
                    break;
                case ":bacon:":
                    _votingClient.VoteBacon(new VoteRequest());
                    break;
                case ":pizza:":
                    _votingClient.VotePizza(new VoteRequest());
                    break;
                case ":taco:":
                    _votingClient.VoteTaco(new VoteRequest());
                    break;
                case ":burrito:":
                    _votingClient.VoteBurrito(new VoteRequest());
                    break;
                case ":ramen:":
                    _votingClient.VoteRamen(new VoteRequest());
                    break;
                case ":doughnut:":
                    _votingClient.VoteDoughnut(new VoteRequest());
                    break;
                case ":champagne:":
                    _votingClient.VoteChampagne(new VoteRequest());
                    break;
                case ":tropical_drink:":
                    _votingClient.VoteTropicalDrink(new VoteRequest());
                    break;
                case ":beer:":
                    _votingClient.VoteBeer(new VoteRequest());
                    break;
                case ":tumbler_glass:":
                    _votingClient.VoteTumblerGlass(new VoteRequest());
                    break;
                case ":world_map:":
                    _votingClient.VoteWorldMap(new VoteRequest());
                    break;
                case ":beach_umbrella:":
                    _votingClient.VoteBeachUmbrella(new VoteRequest());
                    break;
                case ":mountain_snow:":
                    _votingClient.VoteMountainSnow(new VoteRequest());
                    break;
                case ":camping:":
                    _votingClient.VoteCamping(new VoteRequest());
                    break;
                case ":steam_locomotive:":
                    _votingClient.VoteSteamLocomotive(new VoteRequest());
                    break;
                case ":flight_departure:":
                    _votingClient.VoteFlightDeparture(new VoteRequest());
                    break;
                case ":rocket:":
                    _votingClient.VoteRocket(new VoteRequest());
                    break;
                case ":star2:":
                    _votingClient.VoteStar2(new VoteRequest());
                    break;
                case ":sun_behind_small_cloud:":
                    _votingClient.VoteSunBehindSmallCloud(new VoteRequest());
                    break;
                case ":cloud_with_rain:":
                    _votingClient.VoteCloudWithRain(new VoteRequest());
                    break;
                case ":fire:":
                    _votingClient.VoteFire(new VoteRequest());
                    break;
                case ":jack_o_lantern:":
                    _votingClient.VoteJackOLantern(new VoteRequest());
                    break;
                case ":balloon:":
                    _votingClient.VoteBalloon(new VoteRequest());
                    break;
                case ":tada:":
                    _votingClient.VoteTada(new VoteRequest());
                    break;
                case ":trophy:":
                    _votingClient.VoteTrophy(new VoteRequest());
                    break;
                case ":iphone:":
                    _votingClient.VoteIphone(new VoteRequest());
                    break;
                case ":pager:":
                    _votingClient.VotePager(new VoteRequest());
                    break;
                case ":fax:":
                    _votingClient.VoteFax(new VoteRequest());
                    break;
                case ":bulb:":
                    _votingClient.VoteBulb(new VoteRequest());
                    break;
                case ":money_with_wings:":
                    _votingClient.VoteMoneyWithWings(new VoteRequest());
                    break;
                case ":crystal_ball:":
                    _votingClient.VoteCrystalBall(new VoteRequest());
                    break;
                case ":underage:":
                    _votingClient.VoteUnderage(new VoteRequest());
                    break;
                case ":interrobang:":
                    _votingClient.VoteInterrobang(new VoteRequest());
                    break;
                case ":100:":
                    _votingClient.Vote100(new VoteRequest());
                    break;
                case ":checkered_flag:":
                    _votingClient.VoteCheckeredFlag(new VoteRequest());
                    break;
                case ":crossed_swords:":
                    _votingClient.VoteCrossedSwords(new VoteRequest());
                    break;
                case ":floppy_disk:":
                    _votingClient.VoteFloppyDisk(new VoteRequest());
                    break;
            }

            return true;
        }
    }
}