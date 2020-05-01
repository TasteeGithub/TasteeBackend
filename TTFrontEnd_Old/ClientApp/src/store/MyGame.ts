import { Action, Reducer } from 'redux'

//STATE
export interface MyGameState {
    mycount: number;
};


//ACTION
export interface Cong {
    type: 'CONG_COUNT'
}

export interface Tru {
    type: 'TRU_COUNT'
};

export type KnownAction = Cong | Tru;

export const actionCreators = {
    Cong: () => ({ type: 'CONG_COUNT' } as Cong),
    Tru: () => ({ type: 'TRU_COUNT' } as Tru)
};

//REDUCER // noi ket noi action

export const reducer: Reducer<MyGameState> = (state: MyGameState | undefined, incomingAction: Action): MyGameState => {
    if (state === undefined)
        return { mycount: 0 };

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'CONG_COUNT':
            return { mycount: state.mycount + 1 };
        case 'TRU_COUNT':
            return { mycount: state.mycount - 1 };
        default:
            return state;
    }
}

