
export type LanguageType = {
    cultureName: string,
    uiCultureName: string,
    displayName: string,
    twoLetterIsoLanguageName: string,
    flagIcon: string,
}

export type CurrentCultureType = {
    displayName: string,
    englishName: string,
    threeLetterIsoLanguageName: string,
    twoLetterIsoLanguageName: string,
    isRightToLeft: boolean,
    cultureName: string,
    name: string,
    nativeName: string,
    dateTimeFormat: {
      calendarAlgorithmType: string,
      dateTimeFormatLong: string,
      shortDatePattern: string,
      fullDateTimePattern: string,
      dateSeparator: string,
      shortTimePattern: string,
      longTimePattern: string
    }
}

export type AuthType = {
    grantedPolicies: Record<string, boolean>;
}

export type SettingType = {
    values: Record<string, any>;
}

export type CurrentUserType = {
    isAuthenticated: boolean,
    id: string | null,
    tenantId: string | null,
    impersonatorUserId: string | null,
    impersonatorTenantId: string | null,
    impersonatorUserName: string | null,
    impersonatorTenantName: string | null,
    userName: string | null,
    name: string | null,
    surName: string | null,
    email: string | null,
    emailVerified: boolean,
    phoneNumber: string | null,
    phoneNumberVerified: boolean,
    roles: string[],
}

export type FeatureType = {
    values: Record<string, any>;
}

export type GlobalFeaturesType = {
    enabledFeatures: string[];
}

export type MultiTenancyType = {
    isEnabled: boolean;
}

export type currentTenantType = {
    id: string | null,
    name: string | null,
    isAvailable: boolean,
}

export type TimingType = {
    timeZone:{
        iana: {
            timeZoneName: string,
        }
    },
    windows:{
        timeZoneId: string,
    }
}

export type ClockType = {
    kind: string,
}

export type ObjectExtensionsType = {
    modules: Record<string, any>;
    enums: Record<string, any>;
}

export type LocalizationType = {
    defaultResourceName: string;
    currentCulture: CurrentCultureType;
    languages: LanguageType[];
    values: Record<string, any>;
    languagesMap:Record<string, any>;
    languageFilesMap:Record<string, any>;
    resources: Record<string, any>;
}


export type AppConfigType = {
    localization: LocalizationType;
    auth:AuthType;
    setting:SettingType;
    currentUser: CurrentUserType;
    features: FeatureType;
    globalFeatures: GlobalFeaturesType;
    multiTenancy: MultiTenancyType;
    currentTenant:{
        id: string;
        isAvailable: string,
        name: string
    };
    timing: TimingType;
    clock: ClockType;
    objectExtensions: ObjectExtensionsType;
    extraProperties: Record<string, any>;
}
