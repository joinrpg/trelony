import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface CalendarState {
    loadedData:  LoadedRegionPair[];
    games: CalendarRow [];
}

export interface CalendarPageSpec {
    macroRegionId?: number;
    year: number;
}

export interface LoadedRegionPair extends CalendarPageSpec {
    isLoading: boolean;
}

export enum GameStatus {
    ProbablyHappen,
    DefinitelyPassed,
    UnknownStatus,
    PostponedWithoutDate,
    DateUnknown,
    DefinitelyCancelled
}

export enum GameType {
    Forest,
    City,
    OnRentedPlace,
    Room,
    Convention,
    Ball,
    Bugurt,
    CityAndForest,
    CityAndRentedPlace,
    Underground,
    AirsoftEvent,
    Festival
}

export interface CalendarRow {
    gameId: number;
    status: GameStatus;
    name: string;
    vkontakteLink: string;
    startDate: Date;
    gameDurationDays: number;

    subRegionShortName: string;
    macroRegionId: number;

    gameType: GameType;
    polygonName: string;
    playersCount: number;
    organizers: string;
    email: string;
    facebookLink: string;
    telegramLink: string;
    livejournalLink: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestCalendarAction {
    type: 'REQUEST_CALENDAR';
    spec: CalendarPageSpec;
}

interface ReceiveCalendarAction {
    type: 'RECEIVE_CALENDAR';
    spec: CalendarPageSpec;
    games: CalendarRow[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestCalendarAction | ReceiveCalendarAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestCalendar: (year: number, macroRegionId?: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)

        var state = getState().calendar;

        console.log(`requestCalendar(year:${year}, region:${macroRegionId})`);

        console.log(state.loadedData);

        const alreadyPresent = state.loadedData.find(
            e => e.year == year && (e.macroRegionId == undefined || e.macroRegionId == macroRegionId));

        if (alreadyPresent !== undefined)
        {
            console.log(`have it. isLoading: ${alreadyPresent.isLoading}`);
            //already have everything
            return;
        }

        console.log(`sendFetch(year:${year}, region:${macroRegionId}`);

        let fetchTask = fetch(`api/Calendar?year=${year}&macroRegionId=${macroRegionId}`)
            .then(response => response.json() as Promise<CalendarRow[]>)
            .then(data => {
                dispatch({ type: 'RECEIVE_CALENDAR', spec: {macroRegionId: macroRegionId, year: year }, games: data });
            });

        addTask(fetchTask); // Ensure server-side prerendering waits for this to complete
        dispatch({ type: 'REQUEST_CALENDAR', spec:  {macroRegionId: macroRegionId, year: year } });
        }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: CalendarState = { games: [], loadedData: [] };

export const reducer: Reducer<CalendarState> = (state: CalendarState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_CALENDAR':
            let newState1 = {
                ...state,
                loadedData: [
                    ...state.loadedData,
                    {isLoading: true, ...action.spec },
                ]
            };
            return newState1;
        case 'RECEIVE_CALENDAR':
                const newState = {
                    loadedData: getLoadedDataFromRequest(action.spec, action.games, state.loadedData),
                    games: combineGames(state.games, action.games),
                };
                console.log(`After recieve: ${newState}`);
                console.log(newState);
                return newState;
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;
};

const getLoadedDataFromRequest = (spec: CalendarPageSpec, games: CalendarRow[], oldData: LoadedRegionPair[]) : LoadedRegionPair[] =>
{
    const data =  [...oldData];

    const loadingRequest = data.find(s => s.macroRegionId == spec.macroRegionId && s.year == spec.year)!;

    loadingRequest.isLoading = false;

    if (spec.macroRegionId == null)
    {
        const macroRegions = new Set(games.map(g => g.macroRegionId));
        macroRegions.forEach(macroRegionId => {
            data.push({isLoading: false, macroRegionId: macroRegionId, year: spec.year});
        });
    }
    return data;
}

const combineGames = (oldGames: CalendarRow[], newGames: CalendarRow[]) : CalendarRow[] => {
    const games: CalendarRow[] = [];
    oldGames.forEach(g => {
        games[g.gameId] = g;
    })
    newGames.forEach(g => {
        games[g.gameId] = g;
    })
    return games;
}
