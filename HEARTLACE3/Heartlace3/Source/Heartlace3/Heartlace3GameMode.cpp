// Copyright Epic Games, Inc. All Rights Reserved.

#include "Heartlace3GameMode.h"
#include "Heartlace3Character.h"
#include "UObject/ConstructorHelpers.h"

AHeartlace3GameMode::AHeartlace3GameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPerson/Blueprints/BP_FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

}
