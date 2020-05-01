"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
;
;
exports.actionCreators = {
    Cong: function () { return ({ type: 'CONG_COUNT' }); },
    Tru: function () { return ({ type: 'TRU_COUNT' }); }
};
//REDUCER // noi ket noi action
exports.reducer = function (state, incomingAction) {
    if (state === undefined)
        return { mycount: 0 };
    var action = incomingAction;
    switch (action.type) {
        case 'CONG_COUNT':
            return { mycount: state.mycount + 1 };
        case 'TRU_COUNT':
            return { mycount: state.mycount - 1 };
        default:
            return state;
    }
};
//# sourceMappingURL=MyGame.js.map