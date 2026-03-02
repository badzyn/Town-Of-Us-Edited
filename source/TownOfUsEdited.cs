using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Reactor;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using System;
using System.IO;
using System.Reflection;
using TownOfUsEdited.CrewmateRoles.DetectiveMod;
using TownOfUsEdited.CustomOption;
using TownOfUsEdited.Extensions;
using TownOfUsEdited.NeutralRoles.SoulCollectorMod;
using TownOfUsEdited.Patches;
using TownOfUsEdited.RainbowMod;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.CrashReportHandler;

namespace TownOfUsEdited
{
    [BepInPlugin(Id, "Town Of Us Edited Unofficial", VersionString)]
    [BepInDependency(ReactorPlugin.Id)]
    [BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
    [BepInIncompatibility("MalumMenu")]

    public class TownOfUsEdited : BasePlugin
    {
        public const string Id = "com.lekillerdesgames.townofusedited";
        public const string VersionString = "0.0.1";
        public static Version Version = Version.Parse(VersionString);
        public const string VersionTag = "<color=#00F0FF></color>";

        public const int MaxPlayers = 35;

        public static AssetLoader bundledAssets;

        public static Sprite JanitorClean;
        public static Sprite SwitchRole;
        public static Sprite PlaceHolder;
        public static Sprite SabotageCoven;
        public static Sprite Potion;
        public static Sprite Drink;
        public static Sprite Recruit;
        public static Sprite SpiritualistControl;
        public static Sprite Hex;
        public static Sprite HexBomb;
        public static Sprite Freeze;
        public static Sprite Blind;
        public static Sprite Rewind;
        public static Sprite Dissociate;
        public static Sprite Eat;
        public static Sprite Avenge;
        public static Sprite Spell;
        public static Sprite Bounty;
        public static Sprite Curse;
        public static Sprite StoreSprite;
        public static Sprite ManipulateSprite;
        public static Sprite JailSprite;
        public static Sprite PoisonSprite;
        public static Sprite ShiftButton;
        public static Sprite DocReviveButton;
        public static Sprite CapZoomButton;
        public static Sprite EngineerFix;
        public static Sprite SwapperSwitch;
        public static Sprite SwapperSwitchDisabled;
        public static Sprite Footprint;
        public static Sprite MedicSprite;
        public static Sprite SeerSprite;
        public static Sprite SampleSprite;
        public static Sprite Arrow;
        public static Sprite MineSprite;
        public static Sprite DouseSprite;
        public static Sprite IgniteSprite;
        public static Sprite ReviveSprite;
        public static Sprite ButtonSprite;
        public static Sprite DisperseSprite;
        public static Sprite CycleBackSprite;
        public static Sprite CycleForwardSprite;
        public static Sprite GuessSprite;
        public static Sprite DragSprite;
        public static Sprite DropSprite;
        public static Sprite TransformSprite;
        public static Sprite UnTransformSprite;
        public static Sprite FlashSprite;
        public static Sprite AlertSprite;
        public static Sprite RememberSprite;
        public static Sprite PlantSprite;
        public static Sprite DetonateSprite;
        public static Sprite TransportSprite;
        public static Sprite MediateSprite;
        public static Sprite VestSprite;
        public static Sprite BlackmailSprite;
        public static Sprite BlackmailLetterSprite;
        public static Sprite BlackmailOverlaySprite;
        public static Sprite LighterSprite;
        public static Sprite DarkerSprite;
        public static Sprite InfectSprite;
        public static Sprite RampageSprite;
        public static Sprite TrapSprite;
        public static Sprite TrackSprite;
        public static Sprite InspectSprite;
        public static Sprite ExamineSprite;
        public static Sprite EscapeSprite;
        public static Sprite MarkSprite;
        public static Sprite Revive2Sprite;
        public static Sprite ChameleonSwoop;
        public static Sprite ImitateSelectSprite;
        public static Sprite ImitateDeselectSprite;
        public static Sprite ObserveSprite;
        public static Sprite BiteSprite;
        public static Sprite StakeSprite;
        public static Sprite RevealSprite;
        public static Sprite ConfessSprite;
        public static Sprite NoAbilitySprite;
        public static Sprite CamouflageSprite;
        public static Sprite CamoSprintSprite;
        public static Sprite CamoSprintFreezeSprite;
        public static Sprite HackSprite;
        public static Sprite MimicSprite;
        public static Sprite LockSprite;
        public static Sprite SKConvertSprite;
        public static Sprite StalkSprite;
        public static Sprite CrimeSceneSprite;
        public static Sprite CampaignSprite;
        public static Sprite FortifySprite;
        public static Sprite HypnotiseSprite;
        public static Sprite HysteriaSprite;
        public static Sprite InJailSprite;
        public static Sprite ReapSprite;
        public static Sprite SoulSprite;
        public static Sprite Voodoo;
        public static Sprite CampSprite;
        public static Sprite ShootSprite;
        public static Sprite WatchSprite;
        public static Sprite BlessSprite;
        public static Sprite FlushSprite;
        public static Sprite BlockSprite;
        public static Sprite BarricadeSprite;
        public static Sprite BlindSprite;
        public static Sprite GuardSprite;
        public static Sprite BribeSprite;
        public static Sprite BarrierSprite;
        public static Sprite CleanseSprite;
        public static Sprite DetectSprite;
        public static Sprite NoclipSprite;
        public static Sprite AdminSprite;
        public static Sprite JailCellSprite;
        public static Sprite CrusadeSprite;
        public static Sprite SuicideSprite;

        public static Sprite ToUBanner;
        public static Sprite UpdateTOUButton;
        public static Sprite UpdateSubmergedButton;

        public static Sprite ZoomPlusButton;
        public static Sprite ZoomMinusButton;
        public static Sprite ZoomPlusActiveButton;
        public static Sprite ZoomMinusActiveButton;
        public static Sprite NextButton;
        public static Sprite heh;

        public static Vector3 ButtonPosition { get; private set; } = new Vector3(2.6f, 0.7f, -9f);

        private static DLoadImage _iCallLoadImage;

        private Harmony _harmony;
        public static ConfigEntry<bool> DeadSeeGhosts { get; set; }
        public static ConfigEntry<bool> SeeSettingNotifier { get; set; }
        public static ConfigEntry<bool> DisableLobbyMusic { get; set; }
        public static ConfigEntry<bool> Force4Columns { get; set; }
        public static ConfigEntry<bool> ShowWelcomeMessage { get; set; }
        public static ConfigEntry<bool> HideDevStatus { get; set; }
        public static ConfigEntry<bool> DisableTelemetry { get; set; }
        public static ConfigEntry<bool> DontShowCosmeticsInGame { get; set; }

        public static string RuntimeLocation;
        public override void Load()
        {
            RuntimeLocation = Path.GetDirectoryName(Assembly.GetAssembly(typeof(TownOfUsEdited)).Location);
            ReactorCredits.Register<TownOfUsEdited>(ReactorCredits.AlwaysShow);
            System.Console.WriteLine("000.000.000.000/000000000000000000");

            _harmony = new Harmony("com.lekillerdesgames.townofusedited");

            Generate.GenerateAll();

            bundledAssets = new();

            PlaceHolder = CreateSprite("TownOfUsEdited.Resources.Placeholder.png");
            SwitchRole = CreateSprite("TownOfUsEdited.Resources.SwitchRole.png");
            heh = CreateSprite("TownOfUsEdited.Resources.heh.png");
            WatchSprite = CreateSprite("TownOfUsEdited.Resources.Watch.png");
            BlessSprite = CreateSprite("TownOfUsEdited.Resources.Bless.png");
            FlushSprite = CreateSprite("TownOfUsEdited.Resources.Flush.png");
            BlockSprite = CreateSprite("TownOfUsEdited.Resources.Block.png");
            BarricadeSprite = CreateSprite("TownOfUsEdited.Resources.Barricade.png");
            BlindSprite = CreateSprite("TownOfUsEdited.Resources.Blind.png");
            GuardSprite = CreateSprite("TownOfUsEdited.Resources.Guard.png");
            BribeSprite = CreateSprite("TownOfUsEdited.Resources.Bribe.png");
            BarrierSprite = CreateSprite("TownOfUsEdited.Resources.Barrier.png");
            CleanseSprite = CreateSprite("TownOfUsEdited.Resources.Cleanse.png");
            DetectSprite = CreateSprite("TownOfUsEdited.Resources.Detect.png");
            ShootSprite = CreateSprite("TownOfUsEdited.Resources.Shoot.png");
            CampSprite = CreateSprite("TownOfUsEdited.Resources.Camp.png");
            ChameleonSwoop = CreateSprite("TownOfUsEdited.Resources.ChameleonSwoop.png");
            JanitorClean = CreateSprite("TownOfUsEdited.Resources.Clean.png");
            Voodoo = CreateSprite("TownOfUsEdited.Resources.Voodoo.png");
            Potion = CreateSprite("TownOfUsEdited.Resources.Potion.png");
            Drink = CreateSprite("TownOfUsEdited.Resources.Drink.png");
            SabotageCoven = CreateSprite("TownOfUsEdited.Resources.SabotageCoven.png");
            Recruit = CreateSprite("TownOfUsEdited.Resources.Recruit.png");
            SpiritualistControl = CreateSprite("TownOfUsEdited.Resources.Control.png");
            Hex = CreateSprite("TownOfUsEdited.Resources.Hex.png");
            HexBomb = CreateSprite("TownOfUsEdited.Resources.HexBomb.png");
            Freeze = CreateSprite("TownOfUsEdited.Resources.Freeze.png");
            Blind = CreateSprite("TownOfUsEdited.Resources.Blind.png");
            Rewind = CreateSprite("TownOfUsEdited.Resources.Rewind.png");
            Eat = CreateSprite("TownOfUsEdited.Resources.Eat.png");
            Dissociate = CreateSprite("TownOfUsEdited.Resources.Dissociate.png");
            Avenge = CreateSprite("TownOfUsEdited.Resources.Avenge.png");
            TrackSprite = CreateSprite("TownOfUsEdited.Resources.Track.png");
            Spell = CreateSprite("TownOfUsEdited.Resources.Spell.png");
            Curse = CreateSprite("TownOfUsEdited.Resources.Curse.png");
            Bounty = CreateSprite("TownOfUsEdited.Resources.Bounty.png");
            StoreSprite = CreateSprite("TownOfUsEdited.Resources.Store.png");
            ManipulateSprite = CreateSprite("TownOfUsEdited.Resources.ManipulateButton.png");
            JailSprite = CreateSprite("TownOfUsEdited.Resources.JailButton.png");
            PoisonSprite = CreateSprite("TownOfUsEdited.Resources.Poison.png");
            ShiftButton = CreateSprite("TownOfUsEdited.Resources.Shift.png");
            DocReviveButton = CreateSprite("TownOfUsEdited.Resources.DocReviveButton.png");
            CapZoomButton = CreateSprite("TownOfUsEdited.Resources.CapZoom.png");
            EngineerFix = CreateSprite("TownOfUsEdited.Resources.Engineer.png");
            SwapperSwitch = CreateSprite("TownOfUsEdited.Resources.SwapperSwitch.png");
            SwapperSwitchDisabled = CreateSprite("TownOfUsEdited.Resources.SwapperSwitchDisabled.png");
            Footprint = CreateSprite("TownOfUsEdited.Resources.Footprint.png");
            MedicSprite = CreateSprite("TownOfUsEdited.Resources.Medic.png");
            SeerSprite = CreateSprite("TownOfUsEdited.Resources.Seer.png");
            SampleSprite = CreateSprite("TownOfUsEdited.Resources.Sample.png");
            Arrow = CreateSprite("TownOfUsEdited.Resources.Arrow.png");
            MineSprite = CreateSprite("TownOfUsEdited.Resources.Mine.png");
            DouseSprite = CreateSprite("TownOfUsEdited.Resources.Douse.png");
            IgniteSprite = CreateSprite("TownOfUsEdited.Resources.Ignite.png");
            ReviveSprite = CreateSprite("TownOfUsEdited.Resources.Revive.png");
            ButtonSprite = CreateSprite("TownOfUsEdited.Resources.Button.png");
            DisperseSprite = CreateSprite("TownOfUsEdited.Resources.Disperse.png");
            DragSprite = CreateSprite("TownOfUsEdited.Resources.Drag.png");
            DropSprite = CreateSprite("TownOfUsEdited.Resources.Drop.png");
            TransformSprite = CreateSprite("TownOfUsEdited.Resources.Transform.png");
            UnTransformSprite = CreateSprite("TownOfUsEdited.Resources.UnTransform.png");
            CycleBackSprite = CreateSprite("TownOfUsEdited.Resources.CycleBack.png");
            CycleForwardSprite = CreateSprite("TownOfUsEdited.Resources.CycleForward.png");
            GuessSprite = CreateSprite("TownOfUsEdited.Resources.Guess.png");
            FlashSprite = CreateSprite("TownOfUsEdited.Resources.Flash.png");
            AlertSprite = CreateSprite("TownOfUsEdited.Resources.Alert.png");
            RememberSprite = CreateSprite("TownOfUsEdited.Resources.Remember.png");
            PlantSprite = CreateSprite("TownOfUsEdited.Resources.Plant.png");
            DetonateSprite = CreateSprite("TownOfUsEdited.Resources.Detonate.png");
            TransportSprite = CreateSprite("TownOfUsEdited.Resources.Transport.png");
            MediateSprite = CreateSprite("TownOfUsEdited.Resources.Mediate.png");
            VestSprite = CreateSprite("TownOfUsEdited.Resources.Vest.png");
            BlackmailSprite = CreateSprite("TownOfUsEdited.Resources.Blackmail.png");
            BlackmailLetterSprite = CreateSprite("TownOfUsEdited.Resources.BlackmailLetter.png");
            BlackmailOverlaySprite = CreateSprite("TownOfUsEdited.Resources.BlackmailOverlay.png");
            LighterSprite = CreateSprite("TownOfUsEdited.Resources.Lighter.png");
            DarkerSprite = CreateSprite("TownOfUsEdited.Resources.Darker.png");
            InfectSprite = CreateSprite("TownOfUsEdited.Resources.Infect.png");
            RampageSprite = CreateSprite("TownOfUsEdited.Resources.Rampage.png");
            TrapSprite = CreateSprite("TownOfUsEdited.Resources.Trap.png");
            InspectSprite = CreateSprite("TownOfUsEdited.Resources.Inspect.png");
            ExamineSprite = CreateSprite("TownOfUsEdited.Resources.Examine.png");
            EscapeSprite = CreateSprite("TownOfUsEdited.Resources.Mark.png");
            MarkSprite = CreateSprite("TownOfUsEdited.Resources.Mark.png");
            Revive2Sprite = CreateSprite("TownOfUsEdited.Resources.Revive2.png");
            ImitateSelectSprite = CreateSprite("TownOfUsEdited.Resources.ImitateSelect.png");
            ImitateDeselectSprite = CreateSprite("TownOfUsEdited.Resources.ImitateDeselect.png");
            ObserveSprite = CreateSprite("TownOfUsEdited.Resources.Observe.png");
            BiteSprite = CreateSprite("TownOfUsEdited.Resources.Bite.png");
            StakeSprite = CreateSprite("TownOfUsEdited.Resources.Stake.png");
            RevealSprite = CreateSprite("TownOfUsEdited.Resources.Reveal.png");
            ConfessSprite = CreateSprite("TownOfUsEdited.Resources.Confess.png");
            NoAbilitySprite = CreateSprite("TownOfUsEdited.Resources.NoAbility.png");
            CamouflageSprite = CreateSprite("TownOfUsEdited.Resources.Camouflage.png");
            CamoSprintSprite = CreateSprite("TownOfUsEdited.Resources.CamoSprint.png");
            CamoSprintFreezeSprite = CreateSprite("TownOfUsEdited.Resources.CamoSprintFreeze.png");
            HackSprite = CreateSprite("TownOfUsEdited.Resources.Hack.png");
            MimicSprite = CreateSprite("TownOfUsEdited.Resources.Mimic.png");
            LockSprite = CreateSprite("TownOfUsEdited.Resources.Lock.png");
            SKConvertSprite = CreateSprite("TownOfUsEdited.Resources.SKConvert.png");
            StalkSprite = CreateSprite("TownOfUsEdited.Resources.Stalk.png");
            CrimeSceneSprite = CreateSprite("TownOfUsEdited.Resources.CrimeScene.png");
            CampaignSprite = CreateSprite("TownOfUsEdited.Resources.Campaign.png");
            FortifySprite = CreateSprite("TownOfUsEdited.Resources.Fortify.png");
            HypnotiseSprite = CreateSprite("TownOfUsEdited.Resources.Hypnotise.png");
            HysteriaSprite = CreateSprite("TownOfUsEdited.Resources.Hysteria.png");
            InJailSprite = CreateSprite("TownOfUsEdited.Resources.InJail.png");
            ReapSprite = CreateSprite("TownOfUsEdited.Resources.Reap.png");
            SoulSprite = CreateSprite("TownOfUsEdited.Resources.Soul.png");
            NoclipSprite = CreateSprite("TownOfUsEdited.Resources.Noclip.png");
            AdminSprite = CreateSprite("TownOfUsEdited.Resources.Admin.png");
            JailCellSprite = CreateSprite("TownOfUsEdited.Resources.JailCell.png");
            CrusadeSprite = CreateSprite("TownOfUsEdited.Resources.Crusade.png");
            SuicideSprite = CreateSprite("TownOfUsEdited.Resources.Suicide.png");

            ToUBanner = CreateSprite("TownOfUsEdited.Resources.TownOfUsEditedBanner.png");
            UpdateTOUButton = CreateSprite("TownOfUsEdited.Resources.UpdateToUButton.png");
            UpdateSubmergedButton = CreateSprite("TownOfUsEdited.Resources.UpdateSubmergedButton.png");

            ZoomPlusButton = CreateSprite("TownOfUsEdited.Resources.Plus.png");
            ZoomMinusButton = CreateSprite("TownOfUsEdited.Resources.Minus.png");
            ZoomPlusActiveButton = CreateSprite("TownOfUsEdited.Resources.PlusActive.png");
            ZoomMinusActiveButton = CreateSprite("TownOfUsEdited.Resources.MinusActive.png");
            NextButton = CreateSprite("TownOfUsEdited.Resources.NextButton.png");

            PalettePatch.Load();
            ClassInjector.RegisterTypeInIl2Cpp<RainbowBehaviour>();
            ClassInjector.RegisterTypeInIl2Cpp<CrimeScene>();
            ClassInjector.RegisterTypeInIl2Cpp<Soul>();

            // RegisterInIl2CppAttribute.Register();

            DeadSeeGhosts = Config.Bind("Settings", "Dead See Other Ghosts", true, "Whether you see other dead players ghosts while your dead");
            SeeSettingNotifier = Config.Bind("Settings", "See Setting Notifier", true, "Whether you see setting changes in lobby at bottom left");
            DisableLobbyMusic = Config.Bind("Settings", "Disable Lobby Music", false, "Whether you want to disable the lobby Music in-game (can be changed in-game with settings)");
            Force4Columns = Config.Bind("Settings", "Force 4 Columns", false, "Always display 4 columns in meeting, vitals & shapeshifter.");
            ShowWelcomeMessage = Config.Bind("Settings", "Show Welcome Message", true, "Show welcome message after creating a lobby.");
            HideDevStatus = Config.Bind("Settings", "Hide Special Status", false, "Toggle this to hide your special status when launching if you have one. You will still have access to your special perks.");
            DontShowCosmeticsInGame = Config.Bind("Settings", "Dont Show Cosmetics In Game (Client Only)", false, "Don't show any cosmetics in-game (only client-side)");
            DisableTelemetry = Config.Bind("Other", "Disable Telemetry", true, "Prevent the game from collecting analytics and sending them to Innersloth");

            _harmony.PatchAll();

            if (!DisableTelemetry.Value) return;
            Analytics.deviceStatsEnabled = false;
            Analytics.enabled = false;
            Analytics.initializeOnStartup = false;
            Analytics.limitUserTracking = true;
            CrashReportHandler.enableCaptureExceptions = false;
            PerformanceReporting.enabled = false;

            SubmergedCompatibility.Initialize();
            IL2CPPChainloader.Instance.Finished += LevelImpostorCompatibility.Initialize; // LI has a circular dependency on TOU, so we need to wait for LI to finish loading before we can initialize it

            ServerManager.DefaultRegions = new Il2CppReferenceArray<IRegionInfo>(new IRegionInfo[0]);
        }

        public static Sprite CreateSprite(string name)
        {
            var pixelsPerUnit = 100f;
            var pivot = new Vector2(0.5f, 0.5f);
            var assembly = Assembly.GetExecutingAssembly();
            var tex = AmongUsExtensions.CreateEmptyTexture();
            var imageStream = assembly.GetManifestResourceStream(name);
            var img = imageStream.ReadFully();
            LoadImage(tex, img, true);
            tex.DontDestroy();
            var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, pixelsPerUnit);
            sprite.DontDestroy();
            return sprite;
        }

        public static void LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            _iCallLoadImage ??= IL2CPP.ResolveICall<DLoadImage>("UnityEngine.ImageConversion::LoadImage");
            var il2CPPArray = (Il2CppStructArray<byte>)data;
            _iCallLoadImage.Invoke(tex.Pointer, il2CPPArray.Pointer, markNonReadable);
        }

        private delegate bool DLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
    }
}
