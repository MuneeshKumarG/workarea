import styles from '@/styles/DependencyMapping.module.css'
import { ButtonComponent } from '@syncfusion/ej2-react-buttons';
import { ListViewComponent } from '@syncfusion/ej2-react-lists';
import { ComboBoxComponent } from '@syncfusion/ej2-react-dropdowns';
import { TextBoxComponent } from '@syncfusion/ej2-react-inputs';
import { useState } from 'react';
import { useRef } from 'react';
import Image from 'next/image';

let fieldName = "State";
let fieldValues = 
[
    {isMapped:true,name:"TamilNadu"},
    {isMapped:false,name:"AndhraPradesh"},
    {isMapped:false,name:"Kerala"},
    {isMapped:false,name:"Karnataka"},
    {isMapped:false,name:"Telangana"},
  ];

let existingFields = [
    { name:"District", values: [{name:"Chennai",id:1, isMapped:true},
                                {name:"Coimbatore",id:2,isMapped:false},
                                {name:"Madurai",id:3, isMapped:false},
                                {name:"Tiruchirappalli",id:4, isMapped:false},
                                {name:"Tiruppur",id:5, isMapped:true}]},

    { name:"City", values: [{name:"Madurai",id:1, isMapped:false},
                            {name:"Coimbatore",id:2,isMapped:true},
                            {name:"Chennai",id:3, isMapped:false},
                            {name:"Tiruchirappalli",id:4, isMapped:true},
                            {name:"Tiruppur",id:5, isMapped:true}]},

    { name:"Town", values: [{name:"Tiruppur",id:1, isMapped:true},
                            {name:"Tiruchirappalli",id:3, isMapped:true},
                            {name:"Madurai",id:4, isMapped:false},
                            {name:"Coimbatore",id:2,isMapped:true}]}
  ];

  
const DependencyMappingLayout = ({}) => {

    const [isChild, setIsChild] = useState(false);
    const [isParent, setIsParent] = useState(true);
    const [parentMappingdDataSource, setParentMappingdDataSource] = useState(fieldValues);
    const [comboBoxDataSource, setcomboBoxDataSource] = useState(existingFields);
    const [childMappingDataSource, setChildMappingDataSource] = useState([]);
    const comboBoxRef = useRef(null);

    const addParent = () => {
        setIsChild(true);
        setIsParent(false);
        setParentMappingdDataSource ([]);
        setChildMappingDataSource(fieldValues);
        comboBoxRef.current.clear();
    };

    const addChild = () => {
        setIsChild(false);
        setIsParent(true);
        setParentMappingdDataSource (fieldValues);
        setChildMappingDataSource([]);
        comboBoxRef.current.clear();
    };

    const onChange=(args)=> {

        const values=existingFields.find((field) => field.name === args.value).values;

        if(isParent) 
        {
            setChildMappingDataSource(values);
        }
        else
        {
            setParentMappingdDataSource(values);
        }
    }

    return (
        <div>
        <label className={styles.headerLabel}>Map the controlled Fields  and dependent field</label>
        <ButtonComponent cssClass='e-link' className={styles.resetAllButton}>Reset All</ButtonComponent>
        <div className={styles.mappingGridContainer}>
            <MappingActionContainer fieldName={fieldName} 
                              addParent={addParent} 
                              addChild={addChild}/>
            <div className={styles.verticalLine}></div>
            <ParentMappingContainer isParent={isParent} 
                                    parentMappingdDataSource={parentMappingdDataSource}
                                    comboBoxRef={comboBoxRef} 
                                    onChange={onChange}
                                    comboBoxDataSource={comboBoxDataSource} />
            <div className={styles.mappingIconContainer}>
                <Image src="/mapping_double_arrow_icon.png" 
                className={styles.doubleArrowIcon}
                alt='Dependency Mapping Icon'
                height={25} width={25}/>
            </div>
            <ChildMappingComponent isChild={isChild} 
                                   onChange={onChange}
                                   comboBoxDataSource={comboBoxDataSource} 
                                   childMappingDataSource={childMappingDataSource}
                                   comboBoxRef={comboBoxRef}/>
            <SummaryComponent/>
        </div>
        <hr className={styles.horizontalLine}/>
        <div className={styles.footerContainer}>
            <ButtonComponent>Cancel</ButtonComponent>
            <ButtonComponent className={styles.saveButton}>Save</ButtonComponent>
        </div>
        </div>
    );
  };
  
  export default DependencyMappingLayout;

  function MappingActionContainer({fieldName, addParent, addChild}) {
    return (
        <div className={styles.mappingActionContainer}>
            <label className={styles.mappingActionLabel}>{fieldName} is a child of:</label>
            <ButtonComponent className={styles.mappingActionAddParentButton}  
                             onClick={addParent}>+ Add a Parent</ButtonComponent>
            <label className={styles.mappingActionLabel}>{fieldName} is a parent of:</label>
            <ButtonComponent className={styles.mappingActionAddChildButton}
            onClick={addChild}>+ Add a Child (Field Dependency)</ButtonComponent>
        </div>
    );
  }

  function ParentMappingContainer({ isParent, comboBoxRef, onChange ,parentMappingdDataSource, comboBoxDataSource}) {
    return (
        <div className={styles.parentMappingConatiner}>
            <MappingHeaderComponent headerLabel="Parent" isChild={isParent} 
                                onChange={onChange}
                                comboBoxRef={comboBoxRef} 
                                comboBoxDataSource={comboBoxDataSource}/>
            
            <ListViewComponent  id="list" template={parentMappingListViewItemTemplate} 
                                dataSource={parentMappingdDataSource}
            />
        </div>
    );
  }

  function ChildMappingComponent({ isChild , onChange, comboBoxRef, comboBoxDataSource, childMappingDataSource}) {

    return (
        <div className={styles.childMappingContainer}>
            <MappingHeaderComponent headerLabel="Child" isChild={isChild} 
                                onChange={onChange}
                                comboBoxRef={comboBoxRef} 
                                comboBoxDataSource={comboBoxDataSource}/>
            <ListViewComponent dataSource={childMappingDataSource} template={template2}
             cssClass='e-list-template'/>
        </div>
    );
  }

  function MappingHeaderComponent({headerLabel, isChild , onChange, comboBoxRef, comboBoxDataSource}) {

    return (
        <div>
        <div className={styles.mappingHeaderContainer}>
            <label className={styles.mappingHeaderLabel}>{headerLabel} field/Group</label>
            {isChild && 
            <div className={styles.mappingHeaderRoundedContainer}>
                 <div className={styles.mappingHeaderIconContainer}>
                   <Image src="/field_icon.png" 
                className={styles.doubleArrowIcon}
                alt='Field Icon'
                height={15} width={13}/>
                </div>
                <label className={styles.mappingHeaderFieldLabel}>{fieldName}</label>
            </div>}
            {!isChild && (
                    <ComboBoxComponent cssClass={styles.mappingHeaderComboBox}
                                    dataSource={comboBoxDataSource} 
                                    ref={comboBoxRef}
                                    fields={{ text: 'name', value: 'name' }}
                                    onChange={onChange}  
                                    placeholder="Select a Field"/>
                )
            }
            </div>
            <TextBoxComponent  type="text"  cssClass={styles.mappingSearchTextBox}
                   placeholder=" Search" title="Type in a name"/>
                   <hr className={styles.mappingHeaderHorizontalLine}/>
                   </div>
    );
  }
  function SummaryComponent() {
    return (
        <div className={styles.summaryContainer}>
            <div>
                <label className={styles.summaryContainerHeader}>Summary</label>
            </div>
        </div>
    );
  }
  
  function parentMappingListViewItemTemplate(data) {
    return (
         <div className={styles.parentMappingListViewItem} >
            <label className={styles.parentMappingListViewItemLabel} >{data.name}</label>
            { data.isMapped && <label className={styles.parentMappingItemRoundedLabel}>Mapped</label>}
        </div>
        );
    }

    function template2(data) {
        return (
            <div className={styles.childListViewItem}>
                <input type='checkbox'/>
                <label className={styles.childMappingListViewItemLabel}>{data.name}</label>
            </div>
            );
        }
