using AutoMapper;
using EmojiVoting.Application;
using Emojivoto.V1;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmojiVoting.Services
{
    public class VotingGrpcSvc : VotingService.VotingServiceBase
    {
        private readonly ILogger<VotingGrpcSvc> _logger;
        private readonly IPollService _pollService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public VotingGrpcSvc(ILogger<VotingGrpcSvc> logger, IPollService pollService, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _pollService = pollService;
            _mapper = mapper;
            _configuration = configuration;
        }

        private async Task<VoteResponse> Vote(string choice)
        {
            Thread.Sleep(_configuration.GetValue<int>("ARTIFICIAL_DELAY"));
            await _pollService.Vote(choice);
            return new VoteResponse();
        }

        public override async Task<ResultsResponse> Results(ResultsRequest request, ServerCallContext context)
        {
            var votingResults = _mapper.Map<List<VotingResult>>(await _pollService.Results());
            var response = new ResultsResponse();
            response.Results.AddRange(votingResults.AsEnumerable());
            return response;
        }

        public override Task<VoteResponse> VoteDoughnut(VoteRequest request, ServerCallContext context)
        {
            var failureRate = _configuration.GetValue<int>("FAILURE_RATE");
            if (failureRate > 0)
            {
                var random = new Random();
                var probability = random.Next(1, 100);
                if (probability < failureRate)
                {
                    _logger.LogInformation($"probability {probability} is less than failureRate {failureRate}");
                    throw new Exception("logging an error for doughnut");
                }
            }
            _logger.LogInformation("voting for doughnut");
            return Vote(":doughnut:");
        }

        public override Task<VoteResponse> Vote100(VoteRequest request, ServerCallContext context) => Vote(":100:");
        public override Task<VoteResponse> VoteBacon(VoteRequest request, ServerCallContext context) => Vote(":bacon:");
        public override Task<VoteResponse> VoteBalloon(VoteRequest request, ServerCallContext context) => Vote(":balloon:");
        public override Task<VoteResponse> VoteBasketballMan(VoteRequest request, ServerCallContext context) => Vote(":basketball_man:");
        public override Task<VoteResponse> VotePoop(VoteRequest request, ServerCallContext context) => Vote(":poop:");
        public override Task<VoteResponse> VoteJoy(VoteRequest request, ServerCallContext context) => Vote(":joy:");
        public override Task<VoteResponse> VoteSunglasses(VoteRequest request, ServerCallContext context) => Vote(":sunglasses:");
        public override Task<VoteResponse> VoteRelaxed(VoteRequest request, ServerCallContext context) => Vote(":relaxed:");
        public override Task<VoteResponse> VoteStuckOutTongueWinkingEye(VoteRequest request, ServerCallContext context) => Vote(":stuck_out_tongue_winking_eye:");
        public override Task<VoteResponse> VoteMoneyMouthFace(VoteRequest request, ServerCallContext context) => Vote(":money_mouth_face:");
        public override Task<VoteResponse> VoteFlushed(VoteRequest request, ServerCallContext context) => Vote(":flushed:");
        public override Task<VoteResponse> VoteMask(VoteRequest request, ServerCallContext context) => Vote(":mask:");
        public override Task<VoteResponse> VoteNerdFace(VoteRequest request, ServerCallContext context) => Vote(":nerd_face:");
        public override Task<VoteResponse> VoteGhost(VoteRequest request, ServerCallContext context) => Vote(":ghost:");
        public override Task<VoteResponse> VoteSkullAndCrossbones(VoteRequest request, ServerCallContext context) => Vote(":skull_and_crossbones:");
        public override Task<VoteResponse> VoteHeartEyesCat(VoteRequest request, ServerCallContext context) => Vote(":heart_eyes_cat:");
        public override Task<VoteResponse> VoteHearNoEvil(VoteRequest request, ServerCallContext context) => Vote(":hear_no_evil:");
        public override Task<VoteResponse> VoteSeeNoEvil(VoteRequest request, ServerCallContext context) => Vote(":see_no_evil:");
        public override Task<VoteResponse> VoteSpeakNoEvil(VoteRequest request, ServerCallContext context) => Vote(":speak_no_evil:");
        public override Task<VoteResponse> VoteBoy(VoteRequest request, ServerCallContext context) => Vote(":boy:");
        public override Task<VoteResponse> VoteGirl(VoteRequest request, ServerCallContext context) => Vote(":girl:");
        public override Task<VoteResponse> VoteMan(VoteRequest request, ServerCallContext context) => Vote(":man:");
        public override Task<VoteResponse> VoteWoman(VoteRequest request, ServerCallContext context) => Vote(":woman:");
        public override Task<VoteResponse> VoteOlderMan(VoteRequest request, ServerCallContext context) => Vote(":older_man:");
        public override Task<VoteResponse> VotePoliceman(VoteRequest request, ServerCallContext context) => Vote(":policeman:");
        public override Task<VoteResponse> VoteGuardsman(VoteRequest request, ServerCallContext context) => Vote(":guardsman:");
        public override Task<VoteResponse> VoteConstructionWorkerMan(VoteRequest request, ServerCallContext context) => Vote(":construction_worker_man:");
        public override Task<VoteResponse> VotePrince(VoteRequest request, ServerCallContext context) => Vote(":prince:");
        public override Task<VoteResponse> VotePrincess(VoteRequest request, ServerCallContext context) => Vote(":princess:");
        public override Task<VoteResponse> VoteManInTuxedo(VoteRequest request, ServerCallContext context) => Vote(":man_in_tuxedo:");
        public override Task<VoteResponse> VoteBrideWithVeil(VoteRequest request, ServerCallContext context) => Vote(":bride_with_veil:");
        public override Task<VoteResponse> VoteMrsClaus(VoteRequest request, ServerCallContext context) => Vote(":mrs_claus:");
        public override Task<VoteResponse> VoteSanta(VoteRequest request, ServerCallContext context) => Vote(":santa:");
        public override Task<VoteResponse> VoteTurkey(VoteRequest request, ServerCallContext context) => Vote(":turkey:");
        public override Task<VoteResponse> VoteRabbit(VoteRequest request, ServerCallContext context) => Vote(":rabbit:");
        public override Task<VoteResponse> VoteNoGoodWoman(VoteRequest request, ServerCallContext context) => Vote(":no_good_woman:");
        public override Task<VoteResponse> VoteOkWoman(VoteRequest request, ServerCallContext context) => Vote(":ok_woman:");
        public override Task<VoteResponse> VoteRaisingHandWoman(VoteRequest request, ServerCallContext context) => Vote(":raising_hand_woman:");
        public override Task<VoteResponse> VoteBowingMan(VoteRequest request, ServerCallContext context) => Vote(":bowing_man:");
        public override Task<VoteResponse> VoteManFacepalming(VoteRequest request, ServerCallContext context) => Vote(":man_facepalming:");
        public override Task<VoteResponse> VoteWomanShrugging(VoteRequest request, ServerCallContext context) => Vote(":woman_shrugging:");
        public override Task<VoteResponse> VoteMassageWoman(VoteRequest request, ServerCallContext context) => Vote(":massage_woman:");
        public override Task<VoteResponse> VoteWalkingMan(VoteRequest request, ServerCallContext context) => Vote(":walking_man:");
        public override Task<VoteResponse> VoteRunningMan(VoteRequest request, ServerCallContext context) => Vote(":running_man:");
        public override Task<VoteResponse> VoteDancer(VoteRequest request, ServerCallContext context) => Vote(":dancer:");
        public override Task<VoteResponse> VoteManDancing(VoteRequest request, ServerCallContext context) => Vote(":man_dancing:");
        public override Task<VoteResponse> VoteDancingWomen(VoteRequest request, ServerCallContext context) => Vote(":dancing_women:");
        public override Task<VoteResponse> VoteRainbow(VoteRequest request, ServerCallContext context) => Vote(":rainbow:");
        public override Task<VoteResponse> VoteSkier(VoteRequest request, ServerCallContext context) => Vote(":skier:");
        public override Task<VoteResponse> VoteGolfingMan(VoteRequest request, ServerCallContext context) => Vote(":golfing_man:");
        public override Task<VoteResponse> VoteSurfingMan(VoteRequest request, ServerCallContext context) => Vote(":surfing_man:");
        public override Task<VoteResponse> VoteBikingMan(VoteRequest request, ServerCallContext context) => Vote(":biking_man:");
        public override Task<VoteResponse> VotePointUp2(VoteRequest request, ServerCallContext context) => Vote(":point_up_2:");
        public override Task<VoteResponse> VoteVulcanSalute(VoteRequest request, ServerCallContext context) => Vote(":vulcan_salute:");
        public override Task<VoteResponse> VoteMetal(VoteRequest request, ServerCallContext context) => Vote(":metal:");
        public override Task<VoteResponse> VoteCallMeHand(VoteRequest request, ServerCallContext context) => Vote(":call_me_hand:");
        public override Task<VoteResponse> VoteThumbsup(VoteRequest request, ServerCallContext context) => Vote(":thumbsup:");
        public override Task<VoteResponse> VoteWave(VoteRequest request, ServerCallContext context) => Vote(":wave:");
        public override Task<VoteResponse> VoteClap(VoteRequest request, ServerCallContext context) => Vote(":clap:");
        public override Task<VoteResponse> VoteRaisedHands(VoteRequest request, ServerCallContext context) => Vote(":raised_hands:");
        public override Task<VoteResponse> VotePray(VoteRequest request, ServerCallContext context) => Vote(":pray:");
        public override Task<VoteResponse> VoteDog(VoteRequest request, ServerCallContext context) => Vote(":dog:");
        public override Task<VoteResponse> VoteCat2(VoteRequest request, ServerCallContext context) => Vote(":cat2:");
        public override Task<VoteResponse> VotePig(VoteRequest request, ServerCallContext context) => Vote(":pig:");
        public override Task<VoteResponse> VoteHatchingChick(VoteRequest request, ServerCallContext context) => Vote(":hatching_chick:");
        public override Task<VoteResponse> VoteSnail(VoteRequest request, ServerCallContext context) => Vote(":snail:");
        public override Task<VoteResponse> VotePizza(VoteRequest request, ServerCallContext context) => Vote(":pizza:");
        public override Task<VoteResponse> VoteTaco(VoteRequest request, ServerCallContext context) => Vote(":taco:");
        public override Task<VoteResponse> VoteBurrito(VoteRequest request, ServerCallContext context) => Vote(":burrito:");
        public override Task<VoteResponse> VoteRamen(VoteRequest request, ServerCallContext context) => Vote(":ramen:");
        public override Task<VoteResponse> VoteChampagne(VoteRequest request, ServerCallContext context) => Vote(":champagne:");
        public override Task<VoteResponse> VoteTropicalDrink(VoteRequest request, ServerCallContext context) => Vote(":tropical_drink:");
        public override Task<VoteResponse> VoteBeer(VoteRequest request, ServerCallContext context) => Vote(":beer:");
        public override Task<VoteResponse> VoteTumblerGlass(VoteRequest request, ServerCallContext context) => Vote(":tumbler_glass:");
        public override Task<VoteResponse> VoteWorldMap(VoteRequest request, ServerCallContext context) => Vote(":world_map:");
        public override Task<VoteResponse> VoteBeachUmbrella(VoteRequest request, ServerCallContext context) => Vote(":beach_umbrella:");
        public override Task<VoteResponse> VoteMountainSnow(VoteRequest request, ServerCallContext context) => Vote(":mountain_snow:");
        public override Task<VoteResponse> VoteCamping(VoteRequest request, ServerCallContext context) => Vote(":camping:");
        public override Task<VoteResponse> VoteSteamLocomotive(VoteRequest request, ServerCallContext context) => Vote(":steam_locomotive:");
        public override Task<VoteResponse> VoteFlightDeparture(VoteRequest request, ServerCallContext context) => Vote(":flight_departure:");
        public override Task<VoteResponse> VoteRocket(VoteRequest request, ServerCallContext context) => Vote(":rocket:");
        public override Task<VoteResponse> VoteStar2(VoteRequest request, ServerCallContext context) => Vote(":star2:");
        public override Task<VoteResponse> VoteSunBehindSmallCloud(VoteRequest request, ServerCallContext context) => Vote(":sun_behind_small_cloud:");
        public override Task<VoteResponse> VoteCloudWithRain(VoteRequest request, ServerCallContext context) => Vote(":cloud_with_rain:");
        public override Task<VoteResponse> VoteFire(VoteRequest request, ServerCallContext context) => Vote(":fire:");
        public override Task<VoteResponse> VoteJackOLantern(VoteRequest request, ServerCallContext context) => Vote(":jack_o_lantern:");
        public override Task<VoteResponse> VoteTada(VoteRequest request, ServerCallContext context) => Vote(":tada:");
        public override Task<VoteResponse> VoteTrophy(VoteRequest request, ServerCallContext context) => Vote(":trophy:");
        public override Task<VoteResponse> VoteIphone(VoteRequest request, ServerCallContext context) => Vote(":iphone:");
        public override Task<VoteResponse> VotePager(VoteRequest request, ServerCallContext context) => Vote(":pager:");
        public override Task<VoteResponse> VoteFax(VoteRequest request, ServerCallContext context) => Vote(":fax:");
        public override Task<VoteResponse> VoteBulb(VoteRequest request, ServerCallContext context) => Vote(":bulb:");
        public override Task<VoteResponse> VoteMoneyWithWings(VoteRequest request, ServerCallContext context) => Vote(":money_with_wings:");
        public override Task<VoteResponse> VoteCrystalBall(VoteRequest request, ServerCallContext context) => Vote(":crystal_ball:");
        public override Task<VoteResponse> VoteUnderage(VoteRequest request, ServerCallContext context) => Vote(":underage:");
        public override Task<VoteResponse> VoteInterrobang(VoteRequest request, ServerCallContext context) => Vote(":interrobang:");
        public override Task<VoteResponse> VoteCheckeredFlag(VoteRequest request, ServerCallContext context) => Vote(":checkered_flag:");
        public override Task<VoteResponse> VoteCrossedSwords(VoteRequest request, ServerCallContext context) => Vote(":crossed_swords:");
        public override Task<VoteResponse> VoteFloppyDisk(VoteRequest request, ServerCallContext context) => Vote(":floppy_disk:");
    }
}
