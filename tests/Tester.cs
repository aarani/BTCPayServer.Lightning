﻿using BTCPayServer.Lightning.Charge;
using BTCPayServer.Lightning.CLightning;
using BTCPayServer.Lightning.LND;
using NBitcoin;
using NBitcoin.RPC;
using System;
using System.Collections.Generic;
using System.Text;
using BTCPayServer.Lightning.Eclair;

namespace BTCPayServer.Lightning.Tests
{
	public class Tester
	{
		public static NBitcoin.Network Network
		{
			get
			{
				return NBitcoin.Network.RegTest;
			}
		}

		public static RPCClient CreateRPC()
		{
			return new RPCClient("ceiwHEbqWI83:DwubwWsoo3", CommonTests.Docker ? "bitcoind:43782" : "127.0.0.1:37393", Network);
		}

		public static ChargeClient CreateChargeClient()
		{
			return new ChargeClient(new Uri($"http://api-token:foiewnccewuify@{(CommonTests.Docker ? "charge:9112" : "127.0.0.1:37462")}"), Network, allowInsecure: true);
		}

		public static LndClient CreateLndClient()
		{
			return new LndClient(new LndRestSettings()
			{
				AllowInsecure = true,
				Uri = new Uri(CommonTests.Docker ? "http://lnd:8080" : "http://127.0.0.1:32736")
			}, Network.RegTest);
		}

		public static CLightningClient CreateCLightningClient()
		{
			return new CLightningClient(new Uri(CommonTests.Docker ? "tcp://lightningd:9835" : "tcp://127.0.0.1:48532"), Network);
		}

		public static CLightningClient CreateCLightningClientDest()
		{
			return new CLightningClient(new Uri(CommonTests.Docker ? "tcp://lightningd_dest:9835" : "tcp://127.0.0.1:42549"), Network);
		}

		public static EclairLightningClient CreateEclairClient()
		{
			return new EclairLightningClient(new Uri(CommonTests.Docker ? "http://eclair:8080" : "http://127.0.0.1:4570"), "bukkake", Network);
		}

		public static EclairLightningClient CreateEclairClientDest()
		{
			return new EclairLightningClient(new Uri(CommonTests.Docker ? "http://eclair_dest:8080" : "http://127.0.0.1:4571"), "bukkake", Network);
		}

		public static LndClient CreateLndClientDest()
		{
			return new LndClient(new LndRestSettings()
			{
				AllowInsecure = true,
				Uri = new Uri(CommonTests.Docker ? "http://lnd_dest:8080" : "http://127.0.0.1:42802"),
			}, Network.RegTest);
		}

		public static IEnumerable<(string Name, ILightningClient Client)> GetLightningClients()
		{
			yield return ("Charge (Client)", CreateChargeClient());
			yield return ("C-Lightning (Client)", CreateCLightningClient());
			yield return ("LND (Client)", CreateLndClient());
			yield return ("Eclair (Client)", CreateEclairClient());
		}

		public static IEnumerable<(string Name, ILightningClient Customer, ILightningClient Merchant)> GetTestedPairs()
		{
			yield return ("C-Lightning", CreateCLightningClient(), CreateCLightningClientDest());
			yield return ("LND", CreateLndClient(), CreateLndClientDest());
			yield return ("Eclair", CreateEclairClient(), CreateEclairClientDest());
		}
	}
}
