// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class Heartlace3 : ModuleRules
{
	public Heartlace3(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore", "EnhancedInput" });
	}
}
