import { describe, expect, it, vi } from 'vitest';
import { AlertService } from './alert.service';

describe('AlertService confirmation', () => {
  it('waits for acceptance and discards the action when cleared', async () => {
    const service = new AlertService();
    const action = vi.fn();
    service.confirm('¿Guardar?', action, 'Guardar cambios');
    expect(service.alert()).toMatchObject({ type: 'confirmation', title: 'Guardar cambios', message: '¿Guardar?' });
    expect(action).not.toHaveBeenCalled();
    service.clear();
    await service.acceptConfirmation();
    expect(action).not.toHaveBeenCalled();
    expect(service.isOpen()).toBe(false);
  });

  it('consumes the confirmation once and preserves alerts opened by the action', async () => {
    const service = new AlertService();
    let finish!: () => void;
    const pending = new Promise<void>(resolve => { finish = resolve; });
    const action = vi.fn(async () => {
      service.startLoading();
      await pending;
      service.success('Guardado');
    });
    service.confirm('¿Guardar?', action);
    const accepting = service.acceptConfirmation();
    await service.acceptConfirmation();
    expect(action).toHaveBeenCalledTimes(1);
    expect(service.loading()).toBe(true);
    finish();
    await accepting;
    expect(service.alert()?.type).toBe('success');
    expect(service.loading()).toBe(false);
  });

  it('shows an error if the confirmed action rejects', async () => {
    const service = new AlertService();
    service.confirm('¿Continuar?', async () => { throw new Error('Failed'); });
    await service.acceptConfirmation();
    expect(service.alert()?.type).toBe('error');
    expect(service.loading()).toBe(false);
  });

  it('cannot execute an action after another alert replaces the question', async () => {
    const service = new AlertService();
    const action = vi.fn();
    service.confirm('¿Continuar?', action);
    service.error('Operación cancelada');
    await service.acceptConfirmation();
    expect(action).not.toHaveBeenCalled();
    expect(service.alert()?.type).toBe('error');
  });
});
