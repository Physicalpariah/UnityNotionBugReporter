//Created by Matt Purchase.
//  Copyright (c) 2022 Matt Purchase. All rights reserved.
using System.Collections.Generic;
using System;

public class BugReportData {
	// Properties

	// Initalisation Functions

	// Unity Callbacks

	// Public Functions

	public void SendData() {

	}

	// Private Functions

}

[Serializable]
public class parent {
	public string type = "database_id";
	public string database_id;

	public parent(string database_id) {
		this.database_id = database_id;
	}
}

[Serializable]
public class Text {
	public string content;

	public Text(string content) {
		this.content = content;
	}
}

[Serializable]
public class Title {
	public string type = "text";
	public Text text;
	public Title(Text text) {
		this.text = text;
	}
}

[Serializable] // build a bunch of these
public class Name {
	public string type = "title";
	public List<Title> title;

	public Name(List<Title> titles) {
		title = titles;
	}
}

[Serializable]
public class PlainText {
	public string type = "text";
	public Text text;
	public PlainText(Text text) {
		this.text = text;
	}
}

[Serializable]
public class CodeText {
	public string type = "text";
	public Text text;
	public Annotations annotations;
	public CodeText(Text text) {
		this.text = text;
	}
}

[Serializable]
public class Select {
	public string name;

	public Select(string title) {
		name = title;
	}
}

[Serializable]
public class SelectProp {
	public Select select;

	public SelectProp(string title) {
		this.select = new Select(title);
	}
}

[Serializable]
public class Prop {
	public List<PlainText> rich_text;

	public Prop(List<PlainText> titles) {
		rich_text = titles;
	}
}

[Serializable]
public class Paragraph {
	public List<PlainText> rich_text;
	public string language = "javascript";
	public Paragraph(List<PlainText> titles, string title) {
		string heading = title + "\n" + titles[0].text.content + "\n" + "\n";
		titles[0].text.content = heading;
		rich_text = titles;
	}

}


[Serializable]
public class Annotations {
	public bool code = true;
}

[Serializable]
public class Block {
	public string _object = "block";
	public string type = "code";

	public Paragraph code;


	public Block(List<PlainText> titles, string title) {
		code = new Paragraph(titles, title);
	}
}

[Serializable]
public class Language {
	public string language = "JSON";
}

[Serializable]
public class Properties {
	public Name report;
	public SelectProp osVersion;
	public SelectProp deviceType;
	public SelectProp appVersion;
	public SelectProp gameTitle;
	public SelectProp priority;

	public Properties(Name report, SelectProp os, SelectProp device, SelectProp app, SelectProp gameTitle, SelectProp priority) {
		this.report = report;
		this.osVersion = os;
		this.deviceType = device;
		this.appVersion = app;
		this.gameTitle = gameTitle;
		this.priority = priority;
	}
}

[Serializable]
public class Root {
	public parent parent;
	public Properties properties;
	public List<Block> children;

	public Root(parent par, Properties props, List<Block> children) {
		parent = par;
		properties = props;
		this.children = children;
	}
}