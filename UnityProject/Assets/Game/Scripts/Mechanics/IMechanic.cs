using System;

public interface IMechanic {

	event Action MechanicComplete;
	event Action MechanicFailed;
}
