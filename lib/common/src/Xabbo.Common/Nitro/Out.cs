﻿
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Xabbo.Messages.Nitro;

/// <summary>
/// Extracted from Nitro client version WIN63-202410132312-548085391 from BSS Client API.
/// </summary>
public static class Out {

    private static readonly List<Identifier> _allIdentifiers;

    /// <summary>
    /// Gets a read-only list of all Identifier instances defined in this class.
    /// </summary>
    public static IReadOnlyList<Identifier> AllIdentifiers => _allIdentifiers.AsReadOnly();

    // Static constructor to initialize the list using reflection
    static Out()
    {
        _allIdentifiers = new List<Identifier>();
        // Get all public static readonly fields of type Identifier in this class
        FieldInfo[] fields =
            typeof(Out).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        foreach (var field in fields)
        {
            if (field.FieldType == typeof(Identifier))
            {
                // Get the value of the field
                var identifier = (Identifier)field.GetValue(null)!;
                _allIdentifiers.Add(identifier);
            }
        }
    }


    private static Identifier _([CallerMemberName] string? name = null, short? header = null)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        return new Identifier(ClientType.Nitro, Direction.Out, name, header);
    }


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static readonly Identifier Get_Achievement_List = _(header: 3742);
    public static readonly Identifier Bot_Configuration = _(header: 1083);
    public static readonly Identifier Bot_Pickup = _(header: 1783);
    public static readonly Identifier Bot_Place = _(header: 1888);
    public static readonly Identifier Bot_Skill_Save = _(header: 1859);
    public static readonly Identifier Get_Club_Offers = _(header: 999);
    public static readonly Identifier Get_Club_Gift_Info = _(header: 1596);
    public static readonly Identifier Get_Catalog_Index = _(header: 839);
    public static readonly Identifier Get_Catalog_Page = _(header: 85);
    public static readonly Identifier Catalog_Purchase = _(header: 997);
    public static readonly Identifier Catalog_Purchase_Gift = _(header: 451);
    public static readonly Identifier Get_Product_Offer = _(header: 2466);
    public static readonly Identifier Latency = _(header: 1395);
    public static readonly Identifier Latency_Measure = _(header: 2508);
    public static readonly Identifier Pong = _(header: 3294);
    public static readonly Identifier Client_Toolbar_Toggle = _(header: 148);
    public static readonly Identifier Client_Variables = _(header: 1021);
    public static readonly Identifier Get_Current_Timing_Code = _(header: 3612);
    public static readonly Identifier Hotel_News = _(header: 2781);
    public static readonly Identifier Hotel_View = _(header: 2918);
    public static readonly Identifier Get_Bundle_Discount_Ruleset = _(header: 1970);
    public static readonly Identifier Event_Tracker = _(header: 2345);
    public static readonly Identifier Find_New_Friends = _(header: 1005);
    public static readonly Identifier Furniture_Aliases = _(header: 3007);
    public static readonly Identifier Floor_MoveObject = _(header: 1478);
    public static readonly Identifier UseFurniture = _(header: 3299);
    public static readonly Identifier Furniture_Pickup = _(header: 2235);
    public static readonly Identifier Furniture_Place = _(header: 2334);
    public static readonly Identifier Furniture_Postit_Place = _(header: 565);
    public static readonly Identifier Furniture_Postit_Save_Sticky_Pole = _(header: 3055);
    public static readonly Identifier Furniture_Randomstate = _(header: 226);
    public static readonly Identifier Furniture_Wall_Multistate = _(header: 3291);
    public static readonly Identifier Move_WallItem = _(header: 1651);
    public static readonly Identifier Games_Init = _(header: 2376);
    public static readonly Identifier Games_List = _(header: 3955);
    public static readonly Identifier Accept_Game_Invite = _(header: 491);
    public static readonly Identifier Game_Unloaded_Message = _(header: 3857);
    public static readonly Identifier Get_Game_Achievements_Message = _(header: 2392);
    public static readonly Identifier Get_Game_Status_Message = _(header: 2747);
    public static readonly Identifier Get_User_Game_Achievements_Message = _(header: 1773);
    public static readonly Identifier Join_Queue_Message = _(header: 46);
    public static readonly Identifier Leave_Queue_Message = _(header: 949);
    public static readonly Identifier Reset_Resolution_Achievement_Message = _(header: 2276);
    public static readonly Identifier Get_Weekly_Game_Reward_Winners = _(header: 1107);
    public static readonly Identifier Get_Gift_Wrapping_Config = _(header: 776);
    public static readonly Identifier Group_Admin_Add = _(header: 306);
    public static readonly Identifier Group_Admin_Remove = _(header: 585);
    public static readonly Identifier Group_Create_Options = _(header: 2453);
    public static readonly Identifier Group_Favorite = _(header: 230);
    public static readonly Identifier Get_Forum_Stats = _(header: 3361);
    public static readonly Identifier Get_Forum_Threads = _(header: 2683);
    public static readonly Identifier Get_Forums_List = _(header: 1959);
    public static readonly Identifier Get_Forum_Messages = _(header: 3109);
    public static readonly Identifier Get_Forum_Thread = _(header: 2112);
    public static readonly Identifier Get_Unread_Forums_Count = _(header: 2325);
    public static readonly Identifier Forum_Moderate_Message = _(header: 3313);
    public static readonly Identifier Forum_Moderate_Thread = _(header: 2797);
    public static readonly Identifier Forum_Post_Message = _(header: 2199);
    public static readonly Identifier Update_Forum_Read_Marker = _(header: 2246);
    public static readonly Identifier Update_Forum_Settings = _(header: 1409);
    public static readonly Identifier Forum_Update_Thread = _(header: 1159);
    public static readonly Identifier Group_Info = _(header: 1802);
    public static readonly Identifier Group_Delete = _(header: 1984);
    public static readonly Identifier Group_Member_Remove_Confirm = _(header: 837);
    public static readonly Identifier Group_Member_Remove = _(header: 2326);
    public static readonly Identifier Group_Members = _(header: 3310);
    public static readonly Identifier Group_Memberships = _(header: 1105);
    public static readonly Identifier Group_Request = _(header: 1126);
    public static readonly Identifier Group_Request_Accept = _(header: 2292);
    public static readonly Identifier Group_Request_Decline = _(header: 3938);
    public static readonly Identifier Group_Settings = _(header: 217);
    public static readonly Identifier Group_Parts = _(header: 1962);
    public static readonly Identifier Group_Buy = _(header: 3071);
    public static readonly Identifier Group_Save_Information = _(header: 209);
    public static readonly Identifier Group_Save_Badge = _(header: 357);
    public static readonly Identifier Group_Save_Colors = _(header: 235);
    public static readonly Identifier Group_Save_Preferences = _(header: 583);
    public static readonly Identifier Group_Badges = _(header: 1656);
    public static readonly Identifier Inventory_Unknown = _(header: 1940);
    public static readonly Identifier Item_Clothing_Redeem = _(header: 2552);
    public static readonly Identifier Item_Color_Wheel_Click = _(header: 2339);
    public static readonly Identifier Item_Dice_Click = _(header: 447);
    public static readonly Identifier Item_Dice_Close = _(header: 3810);
    public static readonly Identifier Item_Dimmer_Save = _(header: 1989);
    public static readonly Identifier Item_Dimmer_Settings = _(header: 2543);
    public static readonly Identifier Item_Dimmer_Toggle = _(header: 3398);
    public static readonly Identifier Item_Exchange_Redeem = _(header: 3056);
    public static readonly Identifier Item_Paint = _(header: 992);
    public static readonly Identifier Set_Object_Data = _(header: 277);
    public static readonly Identifier Item_Stack_Helper = _(header: 1301);
    public static readonly Identifier Item_Wall_Click = _(header: 3291);
    public static readonly Identifier Item_Wall_Update = _(header: 1651);
    public static readonly Identifier Marketplace_Config = _(header: 1901);
    public static readonly Identifier Accept_Friend = _(header: 1685);
    public static readonly Identifier Messenger_Chat = _(header: 2465);
    public static readonly Identifier Decline_Friend = _(header: 3330);
    public static readonly Identifier Follow_Friend = _(header: 1867);
    public static readonly Identifier Messenger_Friends = _(header: 3845);
    public static readonly Identifier Messenger_Init = _(header: 700);
    public static readonly Identifier Messenger_Relationships = _(header: 1453);
    public static readonly Identifier Set_Relationship_Status = _(header: 1525);
    public static readonly Identifier Remove_Friend = _(header: 1361);
    public static readonly Identifier Request_Friend = _(header: 2025);
    public static readonly Identifier Get_Friend_Requests = _(header: 3230);
    public static readonly Identifier Send_Room_Invite = _(header: 1967);
    public static readonly Identifier HabboSearch = _(header: 3588);
    public static readonly Identifier Friend_List_Update = _(header: 2535);
    public static readonly Identifier Mod_Tool_User_Info = _(header: 3542);
    public static readonly Identifier Get_User_Flat_Cats = _(header: 3210);
    public static readonly Identifier Navigator_Init = _(header: 1429);
    public static readonly Identifier Navigator_Search = _(header: 267);
    public static readonly Identifier Navigator_Search_Close = _(header: 1969);
    public static readonly Identifier Navigator_Search_Open = _(header: 3879);
    public static readonly Identifier Navigator_Search_Save = _(header: 526);
    public static readonly Identifier Get_User_Event_Cats = _(header: 2970);
    public static readonly Identifier Navigator_Settings_Save = _(header: 1537);
    public static readonly Identifier Navigator_Category_List_Mode = _(header: 950);
    public static readonly Identifier Pet_Info = _(header: 2549);
    public static readonly Identifier Pet_Pickup = _(header: 3805);
    public static readonly Identifier Pet_Place = _(header: 172);
    public static readonly Identifier Pet_Respect = _(header: 2078);
    public static readonly Identifier Pet_Ride = _(header: 3911);
    public static readonly Identifier Pet_Move = _(header: 3653);
    public static readonly Identifier Recycler_Prizes = _(header: 54);
    public static readonly Identifier Call_For_Help = _(header: 1335);
    public static readonly Identifier Room_Ambassador_Alert = _(header: 2272);
    public static readonly Identifier Room_Ban_Give = _(header: 2232);
    public static readonly Identifier Room_Ban_List = _(header: 1716);
    public static readonly Identifier Room_Ban_Remove = _(header: 817);
    public static readonly Identifier Room_Create = _(header: 1671);
    public static readonly Identifier Room_Delete = _(header: 876);
    public static readonly Identifier Room_Doorbell = _(header: 1605);
    public static readonly Identifier Room_Enter = _(header: 3631);
    public static readonly Identifier Room_Favorite = _(header: 1383);
    public static readonly Identifier Room_Favorite_Remove = _(header: 1500);
    public static readonly Identifier Can_Create_Room = _(header: 1890);
    public static readonly Identifier Cancel_Room_Event = _(header: 2473);
    public static readonly Identifier Edit_Room_Event = _(header: 3726);
    public static readonly Identifier Competition_Room_Search = _(header: 3974);
    public static readonly Identifier Forward_TRandom_Promoted_Room = _(header: 28);
    public static readonly Identifier Forward_TSome_Room = _(header: 1574);
    public static readonly Identifier Get_Categories_With_User_Count = _(header: 731);
    public static readonly Identifier Get_Guest_Room = _(header: 2191);
    public static readonly Identifier Get_Official_Rooms = _(header: 1215);
    public static readonly Identifier Get_Popular_Room_Tags = _(header: 555);
    public static readonly Identifier Guild_Base_Search = _(header: 2043);
    public static readonly Identifier My_Favourite_Rooms_Search = _(header: 1521);
    public static readonly Identifier My_Frequent_Room_History_Search = _(header: 1320);
    public static readonly Identifier My_Friends_Room_Search = _(header: 1644);
    public static readonly Identifier My_Guild_Bases_Search = _(header: 3123);
    public static readonly Identifier My_Recommended_Rooms = _(header: 2626);
    public static readonly Identifier My_Room_History_Search = _(header: 3427);
    public static readonly Identifier My_Room_Rights_Search = _(header: 3369);
    public static readonly Identifier My_Rooms_Search = _(header: 2988);
    public static readonly Identifier Popular_Rooms_Search = _(header: 26);
    public static readonly Identifier Room_Ad_Event_Tab_Clicked = _(header: 2896);
    public static readonly Identifier Room_Ad_Event_Tab_Viewed = _(header: 1352);
    public static readonly Identifier Room_Ad_Search = _(header: 608);
    public static readonly Identifier Room_Text_Search = _(header: 3582);
    public static readonly Identifier Rooms_Where_My_Friends_Are = _(header: 1875);
    public static readonly Identifier Rooms_With_Highest_Score_Search = _(header: 3321);
    public static readonly Identifier Set_Room_Session_Tags = _(header: 1769);
    public static readonly Identifier Update_Room_Thumbnail = _(header: 2400);
    public static readonly Identifier Room_Kick = _(header: 874);
    public static readonly Identifier Room_Like = _(header: 57);
    public static readonly Identifier Room_Model = _(header: 1889);
    public static readonly Identifier Get_Occupied_Tiles = _(header: 2772);
    public static readonly Identifier Get_Room_Entry_Tile = _(header: 2749);
    public static readonly Identifier Room_Model_Save = _(header: 218);
    public static readonly Identifier Room_Mute = _(header: 282);
    public static readonly Identifier Room_Mute_User = _(header: 1357);
    public static readonly Identifier Room_Rights_Give = _(header: 2731);
    public static readonly Identifier Room_Rights_List = _(header: 1514);
    public static readonly Identifier Room_Rights_Remove = _(header: 2194);
    public static readonly Identifier Room_Rights_Remove_All = _(header: 1788);
    public static readonly Identifier Room_Rights_Remove_Own = _(header: 2530);
    public static readonly Identifier Room_Settings = _(header: 1428);
    public static readonly Identifier Room_Settings_Save = _(header: 2751);
    public static readonly Identifier Room_Staff_Pick = _(header: 3778);
    public static readonly Identifier Security_Machine = _(header: 2819);
    public static readonly Identifier Security_Ticket = _(header: 1461);
    public static readonly Identifier Trade = _(header: 2698);
    public static readonly Identifier Trade_Accept = _(header: 742);
    public static readonly Identifier Trade_Cancel = _(header: 3552);
    public static readonly Identifier Trade_Close = _(header: 2849);
    public static readonly Identifier Trade_Confirm = _(header: 340);
    public static readonly Identifier Trade_Item = _(header: 2618);
    public static readonly Identifier Trade_Item_Remove = _(header: 2992);
    public static readonly Identifier Trade_Items = _(header: 2281);
    public static readonly Identifier Trade_Unaccept = _(header: 2585);
    public static readonly Identifier Action = _(header: 3923);
    public static readonly Identifier Chat = _(header: 2050);
    public static readonly Identifier Shout = _(header: 3044);
    public static readonly Identifier Whisper = _(header: 1445);
    public static readonly Identifier Dance = _(header: 3616);
    public static readonly Identifier Drop_Hand_Item = _(header: 3757);
    public static readonly Identifier Give_Handitem = _(header: 698);
    public static readonly Identifier Look = _(header: 965);
    public static readonly Identifier Posture = _(header: 989);
    public static readonly Identifier Sign = _(header: 1084);
    public static readonly Identifier Typing = _(header: 1755);
    public static readonly Identifier Typing_Stop = _(header: 1580);
    public static readonly Identifier Walk = _(header: 3711);
    public static readonly Identifier User_Badges = _(header: 3148);
    public static readonly Identifier User_Badges_Current = _(header: 954);
    public static readonly Identifier User_Badges_Current_Update = _(header: 915);
    public static readonly Identifier User_Bots = _(header: 1424);
    public static readonly Identifier User_Currency = _(header: 2978);
    public static readonly Identifier User_Effect_Activate = _(header: 644);
    public static readonly Identifier User_Effect_Enable = _(header: 636);
    public static readonly Identifier User_Figure = _(header: 1717);
    public static readonly Identifier User_Request_Furni_Inventory = _(header: 1790);
    public static readonly Identifier User_Home_Room = _(header: 2144);
    public static readonly Identifier User_Info = _(header: 232);
    public static readonly Identifier User_Motto = _(header: 2889);
    public static readonly Identifier User_Ignored = _(header: 609);
    public static readonly Identifier User_Pets = _(header: 1610);
    public static readonly Identifier User_Profile = _(header: 576);
    public static readonly Identifier User_Profile_By_Name = _(header: 3999);
    public static readonly Identifier User_Respect = _(header: 3036);
    public static readonly Identifier Get_Sound_Settings = _(header: 584);
    public static readonly Identifier User_Settings_Camera = _(header: 3919);
    public static readonly Identifier User_Settings_Chat_Style = _(header: 3535);
    public static readonly Identifier User_Settings_Invites = _(header: 1584);
    public static readonly Identifier User_Settings_Old_Chat = _(header: 624);
    public static readonly Identifier User_Settings_Volume = _(header: 553);
    public static readonly Identifier User_Subscription = _(header: 1846);
    public static readonly Identifier Get_Wardrobe = _(header: 1787);
    public static readonly Identifier Save_Wardrobe_Outfit = _(header: 1346);
    public static readonly Identifier User_Tags = _(header: 3339);
    public static readonly Identifier Visit_User = _(header: 3227);
    public static readonly Identifier Wired_Action_Save = _(header: 1150);
    public static readonly Identifier Wired_Apply_Snapshot = _(header: 1857);
    public static readonly Identifier Wired_Condition_Save = _(header: 582);
    public static readonly Identifier Wired_Open = _(header: 176);
    public static readonly Identifier Wired_Trigger_Save = _(header: 1490);
    public static readonly Identifier Get_Item_Data = _(header: 2883);
    public static readonly Identifier One_Way_Door_Click = _(header: 3027);
    public static readonly Identifier Remove_Wall_Item = _(header: 2115);
    public static readonly Identifier Set_Item_Data = _(header: 1297);
    public static readonly Identifier Catalog_Redeem_Voucher = _(header: 3655);
    public static readonly Identifier Room_Toner_Apply = _(header: 110);
    public static readonly Identifier Friend_Furni_Confirm_Lock = _(header: 1674);
    public static readonly Identifier Mannequin_Save_Name = _(header: 1109);
    public static readonly Identifier Mannequin_Save_Look = _(header: 2813);
    public static readonly Identifier Present_Open_Present = _(header: 1833);
    public static readonly Identifier Catalog_Select_Vip_Gift = _(header: 2160);
    public static readonly Identifier User_Ignore_Id = _(header: 1182);
    public static readonly Identifier User_Ignore = _(header: 2640);
    public static readonly Identifier User_Unignore = _(header: 3964);
    public static readonly Identifier Modtool_Request_Room_Info = _(header: 1602);
    public static readonly Identifier Modtool_Change_Room_Settings = _(header: 1397);
    public static readonly Identifier Modtool_Request_User_Chatlog = _(header: 759);
    public static readonly Identifier Modtool_Request_Room_Chatlog = _(header: 2578);
    public static readonly Identifier Modtool_Sanction_Alert = _(header: 2468);
    public static readonly Identifier Modtool_Sanction_Ban = _(header: 1480);
    public static readonly Identifier Modtool_Sanction_Kick = _(header: 3143);
    public static readonly Identifier Modtool_Sanction_Tradelock = _(header: 2403);
    public static readonly Identifier Modtool_Alert_Event = _(header: 206);
    public static readonly Identifier Modtool_Sanction_Mute = _(header: 2493);
    public static readonly Identifier Modtool_Request_User_Rooms = _(header: 3272);
    public static readonly Identifier Modtool_Room_Alert = _(header: 3521);
    public static readonly Identifier Modtool_Preferences = _(header: 2682);
    public static readonly Identifier Close_Issue_Default_Action = _(header: 3673);
    public static readonly Identifier Close_Issues = _(header: 186);
    public static readonly Identifier Default_Sanction = _(header: 675);
    public static readonly Identifier Get_Cfh_Chatlog = _(header: 2163);
    public static readonly Identifier Modtool_Sanction = _(header: 1682);
    public static readonly Identifier Pick_Issues = _(header: 1330);
    public static readonly Identifier Release_Issues = _(header: 2303);
    public static readonly Identifier Convert_Global_Room_Id = _(header: 2987);
    public static readonly Identifier Request_Sell_Item = _(header: 1977);
    public static readonly Identifier Request_Marketplace_Item_Stats = _(header: 3130);
    public static readonly Identifier Marketplace_Sell_Item = _(header: 881);
    public static readonly Identifier Marketplace_Request_Own_Items = _(header: 3328);
    public static readonly Identifier Marketplace_Take_Back_Item = _(header: 1498);
    public static readonly Identifier Marketplace_Redeem_Credits = _(header: 1151);
    public static readonly Identifier Marketplace_Request_Offers = _(header: 3407);
    public static readonly Identifier Marketplace_Buy_Offer = _(header: 3476);
    public static readonly Identifier Marketplace_Buy_Tokens = _(header: 1902);
    public static readonly Identifier Catalog_Request_Pet_Breeds = _(header: 1549);
    public static readonly Identifier Approve_Name = _(header: 3779);
    public static readonly Identifier Unit_Give_Handitem_Pet = _(header: 728);
    public static readonly Identifier Pet_Mount = _(header: 3911);
    public static readonly Identifier Pet_Supplement = _(header: 605);
    public static readonly Identifier Furniture_Group_Info = _(header: 153);
    public static readonly Identifier Achievement_Resolution_Open = _(header: 283);
    public static readonly Identifier Use_Pet_Product = _(header: 264);
    public static readonly Identifier Remove_Pet_Saddle = _(header: 1077);
    public static readonly Identifier Toggle_Pet_Riding = _(header: 3902);
    public static readonly Identifier Toggle_Pet_Breeding = _(header: 2844);
    public static readonly Identifier Unseen_Reset_Category = _(header: 25);
    public static readonly Identifier Unseen_Reset_Items = _(header: 2504);
    public static readonly Identifier Community_Goal_Vote_Composer = _(header: 2696);
    public static readonly Identifier Get_PromArticles = _(header: 2781);
    public static readonly Identifier Accept_Quest = _(header: 3822);
    public static readonly Identifier Activate_Quest = _(header: 1849);
    public static readonly Identifier Cancel_Quest = _(header: 1972);
    public static readonly Identifier Friend_Request_Quest_Complete = _(header: 1922);
    public static readonly Identifier Get_Community_Goal_Earned_Prizes = _(header: 731);
    public static readonly Identifier Get_Community_Goal_Hall_Of_Fame = _(header: 1266);
    public static readonly Identifier Get_Community_Goal_Progress = _(header: 33);
    public static readonly Identifier Get_Concurrent_Users_Goal_Progress = _(header: 703);
    public static readonly Identifier Get_Concurrent_Users_Reward = _(header: 2484);
    public static readonly Identifier Get_Daily_Quest = _(header: 903);
    public static readonly Identifier Get_Quests = _(header: 3468);
    public static readonly Identifier Get_Seasonal_Quests_Only = _(header: 1761);
    public static readonly Identifier Open_Quest_Tracker = _(header: 2234);
    public static readonly Identifier Redeem_Community_Goal_Prize = _(header: 2839);
    public static readonly Identifier Reject_Quest = _(header: 1233);
    public static readonly Identifier Start_Campaign = _(header: 1092);
    public static readonly Identifier Get_Bonus_Rare_Info = _(header: 1186);
    public static readonly Identifier Craft = _(header: 538);
    public static readonly Identifier Craft_Secret = _(header: 1583);
    public static readonly Identifier Get_Craftable_Products = _(header: 3059);
    public static readonly Identifier Get_Crafting_Recipe = _(header: 3347);
    public static readonly Identifier Get_Crafting_Recipes_Available = _(header: 1054);
    public static readonly Identifier PhotCompetition = _(header: 916);
    public static readonly Identifier Publish_Photo = _(header: 3265);
    public static readonly Identifier Purchase_Photo = _(header: 557);
    public static readonly Identifier Render_Room = _(header: 2686);
    public static readonly Identifier Render_Room_Thumbnail = _(header: 90);
    public static readonly Identifier Request_Camera_Configuration = _(header: 3840);
    public static readonly Identifier Add_Jukebox_Disk = _(header: 2472);
    public static readonly Identifier Get_Jukebox_Playlist = _(header: 2841);
    public static readonly Identifier Get_Now_Playing = _(header: 3335);
    public static readonly Identifier Get_Official_Song_Id = _(header: 3131);
    public static readonly Identifier Get_Song_Info = _(header: 3819);
    public static readonly Identifier Get_Sound_Machine_Playlist = _(header: 1675);
    public static readonly Identifier Get_User_Song_Disks = _(header: 2958);
    public static readonly Identifier Remove_Jukebox_Disk = _(header: 190);
    public static readonly Identifier Interstitial_Shown = _(header: 2311);
    public static readonly Identifier Get_Interstitial = _(header: 3077);
    public static readonly Identifier Change_Username = _(header: 3465);
    public static readonly Identifier Check_Username = _(header: 1756);
    public static readonly Identifier Open_Campaign_Calendar_Door_Staff = _(header: 3207);
    public static readonly Identifier Open_Campaign_Calendar_Door = _(header: 2832);
    public static readonly Identifier Builders_Club_Place_Room_Item = _(header: 1798);
    public static readonly Identifier Builders_Club_Place_Wall_Item = _(header: 1418);
    public static readonly Identifier Builders_Club_Query_Furni_Count = _(header: 1408);
    public static readonly Identifier Get_Catalog_Page_Expiration = _(header: 1809);
    public static readonly Identifier Get_Catalog_Page_With_Earliest_Exp = _(header: 3928);
    public static readonly Identifier Get_Direct_Club_Buy_Available = _(header: 713);
    public static readonly Identifier Get_HabbBasic_Membership_Extend_Offer = _(header: 731);
    public static readonly Identifier Get_HabbClub_Extend_Offer = _(header: 1503);
    public static readonly Identifier Get_Is_Offer_Giftable = _(header: 1101);
    public static readonly Identifier Get_Limited_Offer_Appearing_Next = _(header: 3375);
    public static readonly Identifier Get_Next_Targeted_Offer = _(header: 2839);
    public static readonly Identifier Get_Room_Ad_Purchase_Info = _(header: 142);
    public static readonly Identifier Get_Seasonal_Calendar_Daily_Offer = _(header: 2366);
    public static readonly Identifier Get_Targeted_Offer = _(header: 2462);
    public static readonly Identifier Mark_Catalog_New_Additions_Page_Opened = _(header: 3393);
    public static readonly Identifier Purchase_Basic_Membership_Extension = _(header: 1728);
    public static readonly Identifier Purchase_Room_Ad = _(header: 632);
    public static readonly Identifier Purchase_Targeted_Offer = _(header: 49);
    public static readonly Identifier Purchase_Vip_Membership_Extension = _(header: 3943);
    public static readonly Identifier Room_Ad_Purchase_Initiated = _(header: 679);
    public static readonly Identifier Set_Targetted_Offer_State = _(header: 258);
    public static readonly Identifier Shop_Targeted_Offer_Viewed = _(header: 972);
    public static readonly Identifier Helper_Talent_Track = _(header: 2815);
    public static readonly Identifier Forward_TA_Competition_Room = _(header: 3049);
    public static readonly Identifier Forward_TA_Submittable_Room = _(header: 21);
    public static readonly Identifier Forward_TRandom_Competition_Room = _(header: 518);
    public static readonly Identifier Get_Is_User_Part_Of_Competition = _(header: 3046);
    public static readonly Identifier Get_Seconds_Until = _(header: 3234);
    public static readonly Identifier Room_Competition_Init = _(header: 2738);
    public static readonly Identifier Submit_Room_TCompetition = _(header: 3787);
    public static readonly Identifier Vote_For_Room = _(header: 2316);
    public static readonly Identifier Get_Gift = _(header: 731);
    public static readonly Identifier Reset_Phone_Number_State = _(header: 2668);
    public static readonly Identifier Set_Phone_Number_Verification_Status = _(header: 1249);
    public static readonly Identifier Try_Phone_Number = _(header: 1760);
    public static readonly Identifier Verify_Code = _(header: 3154);
    public static readonly Identifier Control_Youtube_Display_Playback = _(header: 3167);
    public static readonly Identifier Get_Youtube_Display_Status = _(header: 1817);
    public static readonly Identifier Set_Youtube_Display_Playlist = _(header: 3762);
    public static readonly Identifier GTFlat = _(header: 497);
    public static readonly Identifier Call_For_Help_From_Forum_Message = _(header: 392);
    public static readonly Identifier Call_For_Help_From_Forum_Thread = _(header: 1785);
    public static readonly Identifier Call_For_Help_From_Im = _(header: 1088);
    public static readonly Identifier Call_For_Help_From_Photo = _(header: 2469);
    public static readonly Identifier Call_For_Help_From_Selfie = _(header: 3514);
    public static readonly Identifier Chat_Review_Guide_Decides = _(header: 2278);
    public static readonly Identifier Chat_Review_Guide_Detached = _(header: 3747);
    public static readonly Identifier Chat_Review_Guide_Vote = _(header: 2221);
    public static readonly Identifier Chat_Review_Session_Create = _(header: 3962);
    public static readonly Identifier Delete_Pending_Calls_For_Help = _(header: 3683);
    public static readonly Identifier Get_Cfh_Status = _(header: 1027);
    public static readonly Identifier Get_Faq_Category = _(header: 713);
    public static readonly Identifier Get_Faq_Text = _(header: 713);
    public static readonly Identifier Get_Guide_Reporting_Status = _(header: 2256);
    public static readonly Identifier Get_Pending_Calls_For_Help = _(header: 318);
    public static readonly Identifier Get_Quiz_Questions = _(header: 2646);
    public static readonly Identifier Guide_Session_Create = _(header: 1268);
    public static readonly Identifier Guide_Session_Feedback = _(header: 607);
    public static readonly Identifier Guide_Session_Get_Requester_Room = _(header: 2282);
    public static readonly Identifier Guide_Session_Guide_Decides = _(header: 647);
    public static readonly Identifier Guide_Session_Invite_Requester = _(header: 3260);
    public static readonly Identifier Guide_Session_Is_Typing = _(header: 2902);
    public static readonly Identifier Guide_Session_Message = _(header: 2915);
    public static readonly Identifier Guide_Session_On_Duty_Update = _(header: 1039);
    public static readonly Identifier Guide_Session_Report = _(header: 506);
    public static readonly Identifier Guide_Session_Requester_Cancels = _(header: 1724);
    public static readonly Identifier Guide_Session_Resolved = _(header: 653);
    public static readonly Identifier Post_Quiz_Answers = _(header: 2372);
    public static readonly Identifier Search_Faqs = _(header: 1664);
    public static readonly Identifier Poll_Answer = _(header: 2503);
    public static readonly Identifier Poll_Reject = _(header: 2722);
    public static readonly Identifier Poll_Start = _(header: 2100);
    public static readonly Identifier Disconnect = _(header: 2740);
    public static readonly Identifier Scr_Get_Kickback_Info = _(header: 3658);
    public static readonly Identifier Compost_Plant = _(header: 2353);
    public static readonly Identifier Harvest_Pet = _(header: 1510);

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
