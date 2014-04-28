using System;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using System.Collections.Generic;
using System.ComponentModel;

namespace ClearInvSSC
{
    [ApiVersion(1, 15)]

    public class ClearInvSSC : TerrariaPlugin
    {
        public override Version Version
        {
            get { return new Version("1.2"); }
        }
        public override string Name
        {
            get { return "ClearInvSSC"; }
        }
        public override string Author
        {
            get { return "IcyPhoenix"; }
        }
        public override string Description
        {
            get { return "Clear Inventory/buffs if SSC Activated"; }
        }
        public ClearInvSSC(Main game)
            : base(game)
        {
            Order = 4;
        }
        public override void Initialize()
        {
            ServerApi.Hooks.NetGetData.Register(this, OnGetData);
            Commands.ChatCommands.Add(new Command(new List<string>(){"clearinv","clearinv.*"}, ResetInventory, "invreset"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ServerApi.Hooks.NetGetData.Deregister(this, OnGetData);
            base.Dispose(disposing);
        }

        private void OnGetData(GetDataEventArgs args)
        {
            if (args.MsgID == PacketTypes.TileGetSection)
            {
                if (Netplay.serverSock[args.Msg.whoAmI].state == 2)
                {
                    CleanInventory(args.Msg.whoAmI);
                }
            }
        }
        private void CleanInventory(int Who)
        {
            if (TShock.Config.ServerSideCharacter && !TShock.Players[Who].IsLoggedIn)
            {
                var player = TShock.Players[Who];
                player.TPlayer.SpawnX = -1;
                player.TPlayer.SpawnY = -1;
                player.sX = -1;
                player.sY = -1;

                for (int i = 0; i < NetItem.maxNetInventory; i++)
                {
                    if (i < NetItem.maxNetInventory - (NetItem.armorSlots + NetItem.dyeSlots))
                    {
                        player.TPlayer.inventory[i].netDefaults(0);
                    }
                    else if (i < NetItem.maxNetInventory - NetItem.dyeSlots)
                    {
                        var index = i - (NetItem.maxNetInventory - (NetItem.armorSlots + NetItem.dyeSlots));
                        player.TPlayer.armor[index].netDefaults(0);
                    }
                    else
                    {
                        var index = i - (NetItem.maxNetInventory - NetItem.dyeSlots);
                        player.TPlayer.dye[index].netDefaults(0);
                    }
                }

                for (int k = 0; k < 59; k++)
                {
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].inventory[k].name, player.Index, (float)k, (float)Main.player[player.Index].inventory[k].prefix, 0f, 0);
                }
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[0].name, player.Index, 59f, (float)Main.player[player.Index].armor[0].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[1].name, player.Index, 60f, (float)Main.player[player.Index].armor[1].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[2].name, player.Index, 61f, (float)Main.player[player.Index].armor[2].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[3].name, player.Index, 62f, (float)Main.player[player.Index].armor[3].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[4].name, player.Index, 63f, (float)Main.player[player.Index].armor[4].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[5].name, player.Index, 64f, (float)Main.player[player.Index].armor[5].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[6].name, player.Index, 65f, (float)Main.player[player.Index].armor[6].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[7].name, player.Index, 66f, (float)Main.player[player.Index].armor[7].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[8].name, player.Index, 67f, (float)Main.player[player.Index].armor[8].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[9].name, player.Index, 68f, (float)Main.player[player.Index].armor[9].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[10].name, player.Index, 69f, (float)Main.player[player.Index].armor[10].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].dye[0].name, player.Index, 70f, (float)Main.player[player.Index].dye[0].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].dye[1].name, player.Index, 71f, (float)Main.player[player.Index].dye[1].prefix, 0f, 0);
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].dye[2].name, player.Index, 72f, (float)Main.player[player.Index].dye[2].prefix, 0f, 0);
                NetMessage.SendData(4, -1, -1, player.Name, player.Index, 0f, 0f, 0f, 0);
                NetMessage.SendData(42, -1, -1, "", player.Index, 0f, 0f, 0f, 0);
                NetMessage.SendData(16, -1, -1, "", player.Index, 0f, 0f, 0f, 0);

                for (int k = 0; k < 59; k++)
                {
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].inventory[k].name, player.Index, (float)k, (float)Main.player[player.Index].inventory[k].prefix, 0f, 0);
                }
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[0].name, player.Index, 59f, (float)Main.player[player.Index].armor[0].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[1].name, player.Index, 60f, (float)Main.player[player.Index].armor[1].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[2].name, player.Index, 61f, (float)Main.player[player.Index].armor[2].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[3].name, player.Index, 62f, (float)Main.player[player.Index].armor[3].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[4].name, player.Index, 63f, (float)Main.player[player.Index].armor[4].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[5].name, player.Index, 64f, (float)Main.player[player.Index].armor[5].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[6].name, player.Index, 65f, (float)Main.player[player.Index].armor[6].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[7].name, player.Index, 66f, (float)Main.player[player.Index].armor[7].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[8].name, player.Index, 67f, (float)Main.player[player.Index].armor[8].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[9].name, player.Index, 68f, (float)Main.player[player.Index].armor[9].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[10].name, player.Index, 69f, (float)Main.player[player.Index].armor[10].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].dye[0].name, player.Index, 70f, (float)Main.player[player.Index].dye[0].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].dye[1].name, player.Index, 71f, (float)Main.player[player.Index].dye[1].prefix, 0f, 0);
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].dye[2].name, player.Index, 72f, (float)Main.player[player.Index].dye[2].prefix, 0f, 0);
                NetMessage.SendData(4, player.Index, -1, player.Name, player.Index, 0f, 0f, 0f, 0);
                NetMessage.SendData(42, player.Index, -1, "", player.Index, 0f, 0f, 0f, 0);
                NetMessage.SendData(16, player.Index, -1, "", player.Index, 0f, 0f, 0f, 0);
            }
        }

        private void ResetInventory(CommandArgs args)
        {
            TSPlayer player = args.Player;
            if (player != null)
            {
                if (TShock.Config.ServerSideCharacter)
                {
                    if (player.Group.HasPermission("clearinv.*"))
                    {
                        player.TPlayer.statLife = 100;
                        player.TPlayer.statLifeMax = 100;
                        player.TPlayer.statMana = 20;
                        player.TPlayer.statManaMax = 20;
                    }
                    for (int i = 0; i < NetItem.maxNetInventory; i++)
                    {
                        if (i < NetItem.maxNetInventory - (NetItem.armorSlots + NetItem.dyeSlots))
                        {
                            player.TPlayer.inventory[i].netDefaults(0);
                        }
                        else if (i < NetItem.maxNetInventory - NetItem.dyeSlots)
                        {
                            var index = i - (NetItem.maxNetInventory - (NetItem.armorSlots + NetItem.dyeSlots));
                            player.TPlayer.armor[index].netDefaults(0);
                        }
                        else
                        {
                            var index = i - (NetItem.maxNetInventory - NetItem.dyeSlots);
                            player.TPlayer.dye[index].netDefaults(0);
                        }
                    }

                    for (int k = 0; k < 59; k++)
                    {
                        NetMessage.SendData(5, -1, -1, Main.player[player.Index].inventory[k].name, player.Index, (float)k, (float)Main.player[player.Index].inventory[k].prefix, 0f, 0);
                    }
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[0].name, player.Index, 59f, (float)Main.player[player.Index].armor[0].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[1].name, player.Index, 60f, (float)Main.player[player.Index].armor[1].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[2].name, player.Index, 61f, (float)Main.player[player.Index].armor[2].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[3].name, player.Index, 62f, (float)Main.player[player.Index].armor[3].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[4].name, player.Index, 63f, (float)Main.player[player.Index].armor[4].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[5].name, player.Index, 64f, (float)Main.player[player.Index].armor[5].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[6].name, player.Index, 65f, (float)Main.player[player.Index].armor[6].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[7].name, player.Index, 66f, (float)Main.player[player.Index].armor[7].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[8].name, player.Index, 67f, (float)Main.player[player.Index].armor[8].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[9].name, player.Index, 68f, (float)Main.player[player.Index].armor[9].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[10].name, player.Index, 69f, (float)Main.player[player.Index].armor[10].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].dye[0].name, player.Index, 70f, (float)Main.player[player.Index].dye[0].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].dye[1].name, player.Index, 71f, (float)Main.player[player.Index].dye[1].prefix, 0f, 0);
                    NetMessage.SendData(5, -1, -1, Main.player[player.Index].dye[2].name, player.Index, 72f, (float)Main.player[player.Index].dye[2].prefix, 0f, 0);
                    NetMessage.SendData(4, -1, -1, player.Name, player.Index, 0f, 0f, 0f, 0);
                    NetMessage.SendData(42, -1, -1, "", player.Index, 0f, 0f, 0f, 0);
                    NetMessage.SendData(16, -1, -1, "", player.Index, 0f, 0f, 0f, 0);

                    for (int k = 0; k < 59; k++)
                    {
                        NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].inventory[k].name, player.Index, (float)k, (float)Main.player[player.Index].inventory[k].prefix, 0f, 0);
                    }
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[0].name, player.Index, 59f, (float)Main.player[player.Index].armor[0].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[1].name, player.Index, 60f, (float)Main.player[player.Index].armor[1].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[2].name, player.Index, 61f, (float)Main.player[player.Index].armor[2].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[3].name, player.Index, 62f, (float)Main.player[player.Index].armor[3].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[4].name, player.Index, 63f, (float)Main.player[player.Index].armor[4].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[5].name, player.Index, 64f, (float)Main.player[player.Index].armor[5].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[6].name, player.Index, 65f, (float)Main.player[player.Index].armor[6].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[7].name, player.Index, 66f, (float)Main.player[player.Index].armor[7].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[8].name, player.Index, 67f, (float)Main.player[player.Index].armor[8].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[9].name, player.Index, 68f, (float)Main.player[player.Index].armor[9].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[10].name, player.Index, 69f, (float)Main.player[player.Index].armor[10].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].dye[0].name, player.Index, 70f, (float)Main.player[player.Index].dye[0].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].dye[1].name, player.Index, 71f, (float)Main.player[player.Index].dye[1].prefix, 0f, 0);
                    NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].dye[2].name, player.Index, 72f, (float)Main.player[player.Index].dye[2].prefix, 0f, 0);
                    NetMessage.SendData(4, player.Index, -1, player.Name, player.Index, 0f, 0f, 0f, 0);
                    NetMessage.SendData(42, player.Index, -1, "", player.Index, 0f, 0f, 0f, 0);
                    NetMessage.SendData(16, player.Index, -1, "", player.Index, 0f, 0f, 0f, 0);

                    if (player.InventorySlotAvailable)
                    {
                        player.GiveItem(-13, "", 0, 0, 1); //copper pickaxe
                        player.GiveItem(-16, "", 0, 0, 1); //copper axe
                        player.GiveItem(-15, "", 0, 0, 1); //copper shortsword
                        player.SendSuccessMessage("Inventory reset to default!");
                    }
                }
            }
        }
    }
}