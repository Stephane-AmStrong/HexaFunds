import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ConnectionTableComponent } from './connection-table.component';
import { provideZonelessChangeDetection } from '@angular/core';


describe('ConnectionTableComponent', () => {
  let component: ConnectionTableComponent;
  let fixture: ComponentFixture<ConnectionTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConnectionTableComponent],
      providers: [
        provideZonelessChangeDetection(),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConnectionTableComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('connections', []);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
