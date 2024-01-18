import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';

export class EmployeeCalculate extends Component {
  static displayName = EmployeeCalculate.name;

  constructor(props) {
    super(props);
    this.state = { id: 0,fullName: '',birthdate: '',tin: '',typeId: 1, days: 0, netIncome: 0, loading: true,loadingCalculate:false };
  }

  componentDidMount() {
      this.getEmployee(this.props.match.params.id);
  }
  handleChange(event) {
    this.setState({ [event.target.name] : event.target.value});
  }

  handleSubmit(e){
      e.preventDefault();
      this.calculateSalary();
  }

  render() {

    let contents = this.state.loading
    ? <p><em>Loading...</em></p>
    : <div>
    <form>
<div className='form-row'>
<div className='form-group col-md-12'>
  <label>Full Name: <b>{this.state.fullName}</b></label>
</div>

</div>

<div className='form-row'>
<div className='form-group col-md-12'>
  <label >Birthdate: <b>{this.state.birthdate}</b></label>
</div>
</div>

<div className="form-row">
<div className='form-group col-md-12'>
  <label>TIN: <b>{this.state.tin}</b></label>
</div>
</div>

<div className="form-row">
<div className='form-group col-md-12'>
  <label>Employee Type: <b>{this.state.typeId === 1?"Regular": "Contractual"}</b></label>
</div>
</div>

{ this.state.typeId === 1?
 <div className="form-row">
     <div className='form-group col-md-12'><label>Salary: 20000 </label></div>
     <div className='form-group col-md-12'><label>Tax: 12% </label></div>
</div> : <div className="form-row">
<div className='form-group col-md-12'><label>Rate Per Day: 500 </label></div>
</div> }

<div className="form-row">
    <div className='form-group col-md-6'>
        { this.state.typeId === 1? 
            <>
            <label htmlFor='inputAbsentDays4'>Absent Days: </label>
            <input type='text' className='form-control' id='inputAbsentDays4' onChange={this.handleChange.bind(this)} value={this.state.days} name="days" placeholder='Absent Days' />
            </>
         :
            <>
            <label htmlFor='inputWorkDays4'>Worked Days: </label>
            <input type='text' className='form-control' id='inputWorkDays4' onChange={this.handleChange.bind(this)} value={this.state.days} name="days" placeholder='Worked Days' />
            </>
        }
    </div>
</div>
<div className="form-row">
    <div className='form-group col-md-12'>
      <label>Net Income: <b>{this.state.netIncome}</b></label>
    </div>
</div>

<button type="submit" onClick={this.handleSubmit.bind(this)} disabled={this.state.loadingCalculate} className="btn btn-primary mr-2">{this.state.loadingCalculate?"Loading...": "Calculate"}</button>
<button type="button" onClick={() => this.props.history.push("/employees/index")} className="btn btn-primary">Back</button>
</form>
</div>;


    return (
        <div>
        <h1 id="tabelLabel" >Employee Calculate Salary</h1>
        <br/>
        {contents}
      </div>
    );
  }

  async calculateSalary() {
      this.setState({ loadingCalculate: true });

      const decimalDays = parseFloat(this.state.days);

      console.log(this.state);

    const token = await authService.getAccessToken();
    const requestOptions = {
        method: 'POST',
        headers: !token ? {} : { 'Authorization': `Bearer ${token}`,'Content-Type': 'application/json' },
        body: JSON.stringify({ id: this.state.id, days: decimalDays})
    };
      const response = await fetch('api/employees/' + this.state.id + '/calculate', requestOptions);

      if (response.status === 200) {
          const data = await response.json();
          this.setState({ loadingCalculate: false, netIncome: data.toFixed(2) });
      } else if (response.status === 400) {

          const responseData = await response.json();
          console.log(responseData);


          const errors = responseData.errors;
          if (errors && errors.length > 0) {
              const errorMessages = errors.map(error => `- ${error}`).join('\n');
              alert("There was an issue with your inputs:\n" + errorMessages);
          } else {
              alert("There was an error occurred.");
          }

          this.setState({ loadingCalculate: false });
      }
      
  }

  async getEmployee(id) {
    this.setState({ loading: true,loadingCalculate: false });
    const token = await authService.getAccessToken();
    const response = await fetch('api/employees/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    if(response.status === 200){
        const data = await response.json();
        this.setState({ id: data.id,fullName: data.fullName,birthdate: data.birthdate,tin: data.tin,typeId: data.typeId, loading: false,loadingCalculate: false });
    } else if (response.status === 400) {

        const responseData = await response.json();
        console.log(responseData);


        const errors = responseData.errors;
        if (errors && errors.length > 0) {
            const errorMessages = errors.map(error => `- ${error}`).join('\n');
            alert("There was an issue with your inputs:\n" + errorMessages);
        } else {
            alert("There was an error occurred.");
        }

        this.setState({ loadingCalculate: false, netIncome: 0 });
    }
    else{
        alert("There was an error occured.");
        this.setState({ loading: false, loadingCalculate: false, netIncome: 0 });
        this.props.history.push('/employees/index');
    }
  }
}
