import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as CalendarState from '../store/Calendar';

export type MacroRegionString = 'all' | 'spb' | 'msk';

// At runtime, Redux will merge together...
type CalendarProps =
    CalendarState.CalendarState        // ... state we've requested from the Redux store
    & typeof CalendarState.actionCreators      // ... plus action creators we've requested
    & RouteComponentProps<{ year: string, regionStr: MacroRegionString }>; // ... plus incoming routing parameters

class CalendarContainer extends React.Component<CalendarProps, {}> {

    updateProps(nextProps: CalendarProps) {
        // This method runs when the component is first added to the page
        let year = parseInt(nextProps.match.params.year) || 0;
        let macroRegionId: number | undefined = undefined;
        switch (nextProps.match.params.regionStr){
            case 'all':
                macroRegionId = undefined;
                break;
            case 'spb':
                macroRegionId = 1;
                break;
            case 'msk':
                macroRegionId = 2;
                break;
        }

        nextProps.requestCalendar(year, macroRegionId);
    }

    componentWillMount() {
       this.updateProps(this.props);
    }

    componentWillReceiveProps(nextProps: CalendarProps) {
        console.log("componentWillReceiveProps");
        console.log(nextProps);
        this.updateProps(nextProps);
    }

    public render() {
        console.log("render");
        return <div>
            <h1>Calendar</h1>
            <p>This component demonstrates fetching data from the server and working with URL parameters.</p>
            { this.renderCalendarTable() }
        </div>;
    }

    //TODO move rendering to stateless component
    private renderCalendarTable() {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Name</th>
                </tr>
            </thead>
            <tbody>
            {this.props.games.map(game =>
                <tr key={ game.gameId }>
                    <td>{ game.name }</td>
                </tr>
            )}
            </tbody>
        </table>;
    }

  /*  private renderPagination() {
        let prevStartDateIndex = (this.props.startDateIndex || 0) - 5;
        let nextStartDateIndex = (this.props.startDateIndex || 0) + 5;

        return <p className='clearfix text-center'>
            <Link className='btn btn-default pull-left' to={ `/calendar/${ prevStartDateIndex }` }>Previous</Link>
            <Link className='btn btn-default pull-right' to={ `/calendar/${ nextStartDateIndex }` }>Next</Link>
            { this.props.isLoading ? <span>Loading...</span> : [] }
        </p>;
    }*/
}

export default connect(
    (state: ApplicationState) => state.calendar, // Selects which state properties are merged into the component's props
    CalendarState.actionCreators                 // Selects which action creators are merged into the component's props
)(CalendarContainer) as typeof CalendarContainer;
