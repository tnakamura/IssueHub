﻿using Xamarin.Forms;

namespace IssueHub.Utils
{
    public static class Octicons
    {
        public enum Glyph
        {
            Alert = 61696,
            Archive = 61697,
            ArrowBoth = 61698,
            ArrowDown = 61699,
            ArrowLeft = 61700,
            ArrowRight = 61701,
            ArrowSmallDown = 61702,
            ArrowSmallLeft = 61703,
            ArrowSmallRight = 61704,
            ArrowSmallUp = 61705,
            ArrowUp = 61706,
            Beaker = 61707,
            Bell = 61708,
            Bold = 61709,
            Book = 61710,
            Bookmark = 61711,
            Briefcase = 61712,
            Broadcast = 61713,
            Browser = 61714,
            Bug = 61715,
            Calendar = 61716,
            Check = 61717,
            Checklist = 61718,
            ChevronDown = 61719,
            ChevronLeft = 61720,
            ChevronRight = 61721,
            ChevronUp = 61722,
            CircleSlash = 61723,
            CircuitBoard = 61724,
            Clippy = 61725,
            Clock = 61726,
            CloudDownload = 61727,
            CloudUpload = 61728,
            Code = 61729,
            Comment = 61730,
            CommentDiscussion = 61731,
            CreditCard = 61732,
            Dash = 61733,
            Dashboard = 61734,
            Database = 61735,
            DesktopDownload = 61736,
            DeviceCamera = 61737,
            DeviceCameraVideo = 61738,
            DeviceDesktop = 61739,
            DeviceMobile = 61740,
            Diff = 61741,
            DiffAdded = 61742,
            DiffIgnored = 61743,
            DiffModified = 61744,
            DiffRemoved = 61745,
            DiffRenamed = 61746,
            Ellipsis = 61747,
            Eye = 61748,
            EyeClosed = 61749,
            File = 61750,
            FileBinary = 61751,
            FileCode = 61752,
            FileDirectory = 61753,
            FileMedia = 61754,
            FilePdf = 61755,
            FileSubmodule = 61756,
            FileSymlinkDirectory = 61757,
            FileSymlinkFile = 61758,
            FileZip = 61759,
            Flame = 61760,
            Fold = 61761,
            FoldDown = 61762,
            FoldUp = 61763,
            Gear = 61764,
            Gift = 61765,
            Gist = 61766,
            GistSecret = 61767,
            GitBranch = 61768,
            GitCommit = 61769,
            GitCompare = 61770,
            GitMerge = 61771,
            GitPullRequest = 61772,
            GithubAction = 61773,
            Globe = 61774,
            Grabber = 61775,
            Graph = 61776,
            Heart = 61777,
            History = 61778,
            Home = 61779,
            HorizontalRule = 61780,
            Hubot = 61781,
            Inbox = 61782,
            Info = 61783,
            IssueClosed = 61784,
            IssueOpened = 61785,
            IssueReopened = 61786,
            Italic = 61787,
            Jersey = 61788,
            KebabHorizontal = 61789,
            KebabVertical = 61790,
            Key = 61791,
            Keyboard = 61792,
            Law = 61793,
            LightBulb = 61794,
            Link = 61795,
            LinkExternal = 61796,
            ListOrdered = 61797,
            ListUnordered = 61798,
            Location = 61799,
            Lock = 61800,
            LogoGist = 61801,
            LogoGithub = 61802,
            Mail = 61803,
            MailRead = 61804,
            MarkGithub = 61805,
            Markdown = 61806,
            Megaphone = 61807,
            Mention = 61808,
            Milestone = 61809,
            Mirror = 61810,
            MortarBoard = 61811,
            Mute = 61812,
            NoNewline = 61813,
            Note = 61814,
            Octoface = 61815,
            Organization = 61816,
            Package = 61817,
            Paintcan = 61818,
            Pencil = 61819,
            Person = 61820,
            Pin = 61821,
            Play = 61822,
            Plug = 61823,
            Plus = 61824,
            PlusSmall = 61825,
            PrimitiveDot = 61826,
            PrimitiveSquare = 61827,
            Project = 61828,
            Pulse = 61829,
            Question = 61830,
            Quote = 61831,
            RadioTower = 61832,
            Reply = 61833,
            Repo = 61834,
            RepoClone = 61835,
            RepoForcePush = 61836,
            RepoForked = 61837,
            RepoPull = 61838,
            RepoPush = 61839,
            Report = 61840,
            RequestChanges = 61841,
            Rocket = 61842,
            Rss = 61843,
            Ruby = 61844,
            ScreenFull = 61845,
            ScreenNormal = 61846,
            Search = 61847,
            Server = 61848,
            Settings = 61849,
            Shield = 61850,
            SignIn = 61851,
            SignOut = 61852,
            Smiley = 61853,
            Squirrel = 61854,
            Star = 61855,
            Stop = 61856,
            Sync = 61857,
            Tag = 61858,
            Tasklist = 61859,
            Telescope = 61860,
            Terminal = 61861,
            TextSize = 61862,
            ThreeBars = 61863,
            Thumbsdown = 61864,
            Thumbsup = 61865,
            Tools = 61866,
            Trashcan = 61867,
            TriangleDown = 61868,
            TriangleLeft = 61869,
            TriangleRight = 61870,
            TriangleUp = 61871,
            Unfold = 61872,
            Unmute = 61873,
            Unverified = 61874,
            Verified = 61875,
            Versions = 61876,
            Watch = 61877,
            X = 61878,
            Zap = 61879,
        }

        static readonly string FontFamily = Device.RuntimePlatform == Device.Android
            ? "Octicons.ttf#Octicons"
            : "Octicons";

        public static FontImageSource GetImageSource(Glyph glyph, double size)
        {
            return new FontImageSource
            {
                FontFamily = FontFamily,
                Glyph = GetGlyphString(glyph),
                Size = size,
            };
        }

        public static string GetGlyphString(Glyph glyph) =>
            ((char)(int)glyph).ToString();
    }
}
