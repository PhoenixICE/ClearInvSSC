using System;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using System.Collections.Generic;
using System.ComponentModel;

namespace ClearInvSSC
{
	[ApiVersion(1, 16)]

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
			if (TShock.ServerSideCharacterConfig.Enabled && !TShock.Players[Who].IsLoggedIn)
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

				for (int k = 0; k < NetItem.maxNetInventory; k++)
				{
					NetMessage.SendData(5, -1, -1, "", player.Index, (float)k, 0f, 0f, 0);
				}

				for (int k = 0; k < Player.maxBuffs; k++)
				{
					player.TPlayer.buffType[k] = 0;
				}

				NetMessage.SendData(4, -1, -1, player.Name, player.Index, 0f, 0f, 0f, 0);
				NetMessage.SendData(42, -1, -1, "", player.Index, 0f, 0f, 0f, 0);
				NetMessage.SendData(16, -1, -1, "", player.Index, 0f, 0f, 0f, 0);
				NetMessage.SendData(50, -1, -1, "", player.Index, 0f, 0f, 0f, 0);

				for (int k = 0; k < NetItem.maxNetInventory; k++)
				{
					NetMessage.SendData(5, player.Index, -1, "", player.Index, (float)k, 0f, 0f, 0);
				}

				for (int k = 0; k < Player.maxBuffs; k++)
				{
					player.TPlayer.buffType[k] = 0;
				}

				NetMessage.SendData(4, player.Index, -1, player.Name, player.Index, 0f, 0f, 0f, 0);
				NetMessage.SendData(42, player.Index, -1, "", player.Index, 0f, 0f, 0f, 0);
				NetMessage.SendData(16, player.Index, -1, "", player.Index, 0f, 0f, 0f, 0);
				NetMessage.SendData(50, player.Index, -1, "", player.Index, 0f, 0f, 0f, 0);

			}
		}
	}
}