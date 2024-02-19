// Copyright Epic Games, Inc. All Rights Reserved.

#include "Heartlace2GameMode.h"
#include "Heartlace2Character.h"
#include "UObject/ConstructorHelpers.h"

AHeartlace2GameMode::AHeartlace2GameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPerson/Blueprints/BP_FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

}
