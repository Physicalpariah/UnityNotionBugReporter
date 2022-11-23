//Created by Matt Purchase.
//  Copyright (c) 2022 Matt Purchase. All rights reserved.
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ErrorLogGathererTests {
	// Properties
	private ErrorLogGatherer m_gatherer;

	// Tests
	[SetUp]
	public void Setup() {
		m_gatherer = new ErrorLogGatherer();
		m_gatherer.Initialise();
	}

	[TearDown]
	public void TearDown() {
		m_gatherer = null;
	}

	[Test]
	public void WillClearLogs() {

		m_gatherer.m_logs.Add("Oh damn");

		m_gatherer.ClearLogs();

		if (m_gatherer.m_logs.Count == 0) {
			Assert.Pass();
		}

		Assert.Fail();
	}

	[Test]
	public void WillGatherLogs() {
		m_gatherer.m_logs.Add("Oh damn");

		string logs = m_gatherer.GetLogs();

		foreach (string log in m_gatherer.m_logs) {
			if (log.Contains("damn")) {
				Assert.Pass();
			}
		}

		Assert.Fail();
	}
}