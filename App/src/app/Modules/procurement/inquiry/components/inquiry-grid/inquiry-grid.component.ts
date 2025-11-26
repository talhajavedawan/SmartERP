import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridApi, CellEditingStartedEvent, CellEditingStoppedEvent } from 'ag-grid-community';

export interface InquiryItem {
  id: number;
  product: Product;
  itemDescription: string;
  ownDiscription: string;
  unitOfMeasure: string;
  weight: number;
  quantity: number;
}

export interface Product {
  code: string;
  item: string;
  itemDescription: string;
  unitOfMeasure: UnitOfMeasure;
  nature: Nature;
  category: Category;
}

export interface UnitOfMeasure {
  unitOfMeasure: string;
}

export interface Nature {
  nature: string;
}

export interface Category {
  category: string;
}

@Component({
  selector: 'app-inquiry-grid',
  standalone: true,
  imports: [
    CommonModule,
    AgGridAngular
  ],
  template: `
    <div class="ag-theme-alpine" style="height: 400px; width: 100%;">
      <ag-grid-angular
        #agGrid
        style="width: 100%; height: 100%"
        class="ag-theme-alpine"
        [rowData]="rowData"
        [columnDefs]="columnDefs"
        [defaultColDef]="defaultColDef"
        [rowSelection]="'multiple'"
        [animateRows]="true"
        [pagination]="false"
        [paginationPageSize]="10"
        [suppressCellFocus]="false"
        [stopEditingWhenCellsLoseFocus]="true"
        [rowDragManaged]="false"
        [enableCellTextSelection]="true"
        [domLayout]="'normal'"
        
        [columnDefs]="columnDefs"
        
        (gridReady)="onGridReady($event)"
      >
      </ag-grid-angular>
    </div>
  `,
  styles: [`
    :host {
      display: block;
      height: 100%;
      width: 100%;
    }
    
    .ag-theme-alpine {
      --ag-header-background-color: #f0f8ff;
      --ag-header-foreground-color: #000080;
      --ag-row-height: 32px;
      --ag-font-size: 14px;
      width: 100% !important;
    }
  `]
})
export class InquiryGridComponent implements OnInit {
  @ViewChild('agGrid') agGrid!: AgGridAngular;
  private gridApi!: GridApi;

  rowData: InquiryItem[] = [
    {
      id: 1,
      product: {
        code: 'ITEM-001',
        item: 'Sample Item 1',
        itemDescription: 'Description for item 1',
        unitOfMeasure: { unitOfMeasure: 'Each' },
        nature: { nature: 'Material' },
        category: { category: 'Electronics' }
      },
      itemDescription: 'Sample item description',
      ownDiscription: 'Custom description',
      unitOfMeasure: 'Each',
      weight: 1.5,
      quantity: 10
    },
    {
      id: 2,
      product: {
        code: 'ITEM-002',
        item: 'Sample Item 2',
        itemDescription: 'Description for item 2',
        unitOfMeasure: { unitOfMeasure: 'Kg' },
        nature: { nature: 'Material' },
        category: { category: 'Hardware' }
      },
      itemDescription: 'Another sample item',
      ownDiscription: 'Another custom description',
      unitOfMeasure: 'Kg',
      weight: 2.3,
      quantity: 5
    }
  ];


  // This component is designed to display and edit inquiry items
  columnDefs: ColDef[] = [
    {
      // ID column for internal use only, will not be visible to user
      field: 'id',
      headerName: 'Id',
      hide: true,
      editable: false
    },
    {
      // Item column where user can select items
      field: 'product.code',
      headerName: 'Item',
      editable: true,
      cellEditor: 'agSelectCellEditor',
      cellEditorParams: {
        values: ['ITEM-001', 'ITEM-002', 'ITEM-003', 'ITEM-004', 'ITEM-005']
      },
      valueFormatter: (params) => {
        return params.data?.product?.code || '';
      },
      flex: 1,
      minWidth: 120
    },
    {
      // Item detail which is the product description
      field: 'itemDescription',
      headerName: 'Item Description',
      editable: true,
      flex: 1,
      minWidth: 150
    },
    {
      // User's own description about the product
      field: 'ownDiscription',
      headerName: 'Own Description',
      editable: true,
      flex: 1,
      minWidth: 150
    },
    {
      // Unit of measure like kg, g, etc.
      field: 'unitOfMeasure',
      headerName: 'Unit Of Measure',
      editable: true,
      cellEditor: 'agSelectCellEditor',
      cellEditorParams: {
        values: ['Each', 'Kg', 'Gram', 'Liter', 'Meter', 'Box', 'Set']
      },
      flex: 1,
      minWidth: 120
    },
    {
      // Weight column for numeric input
      field: 'weight',
      headerName: 'Weight',
      editable: true,
      cellEditor: 'agNumberCellEditor',
      cellEditorParams: {
        precision: 2,
        min: 0
      },
      valueFormatter: (params) => {
        return params.value ? parseFloat(params.value).toFixed(2) : '0.00';
      },
      flex: 0.5,
      minWidth: 100
    },
    {
      // Quantity column for numeric input
      field: 'quantity',
      headerName: 'Quantity',
      editable: true,
      cellEditor: 'agNumberCellEditor',
      cellEditorParams: {
        precision: 2,
        min: 1,
        max: 99999
      },
      valueFormatter: (params) => {
        return params.value ? parseFloat(params.value).toFixed(2) : '0.00';
      },
      flex: 0.5,
      minWidth: 100
    }
  ];

  defaultColDef: ColDef = {
    sortable: true,
    filter: true,
    resizable: true,
    editable: true,
    minWidth: 100,
  };

  // Enable column sizing to fit the grid width
  // onGridReady(params: any): void {
  //   this.gridApi = params.api;
    
  //   // Size columns to fit the available width after a short delay
  //   setTimeout(() => {
  //     if (this.gridApi) {
  //       this.gridApi.sizeColumnsToFit({ 
  //         defaultMinWidth: 100,
  //         defaultMaxWidth: 300
  //       });
  //     }
  //   }, 100);
  // }

  ngOnInit(): void {
    // کمپونینٹ کی شروعاتی تیاری
    // Component initialization
  }

  onGridReady(params: any): void {
    this.gridApi = params.api;
  }
}