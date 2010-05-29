﻿namespace gar3t.LucidIoC
{
	public class ResolutionContext
	{
		private readonly ResolutionInfo _resolutionInfo;

		public ResolutionContext(ResolutionInfo resolutionInfo)
		{
			_resolutionInfo = resolutionInfo;
		}

		public ResolutionContext AsSingleton()
		{
			_resolutionInfo.IsSingleton = true;
			return this;
		}
	}
}