using System;

public interface IMultipleExecution {
	event Action<int> TimesExecutedChanged;
}
